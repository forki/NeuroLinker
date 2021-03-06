﻿using FluentAssertions;
using Moq;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces;
using NeuroLinker.Workers;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.ResponseWrappers;

namespace NeuroLinker.Tests.Workers
{
    public class ListRetrievalWorkerTests
    {
        #region Public Methods

        [Test]
        public void RetrievingListCorrectlyRetrievesAnimeInformation()
        {
            // arrange
            const string user = "test";
            var fixture = new ListRetrievalWorkerFixture();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", "UserList.xml");
            var data = File.ReadAllText(examplePath);
            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveDocumentAsStringAsync(MalRouteBuilder.UserListUrl(user)))
                .ReturnsAsync(new StringRetrievalWrapper(HttpStatusCode.OK, true, data));

            var sut = fixture.Instance;

            // act
            var result = sut.RetrieveUserListAsync(user).Result;

            // assert
            result.Anime.Count.Should().Be(2);
        }

        [Test]
        public void RetrievingListCorrectlyRetrievesUserInformation()
        {
            // arrange
            const string user = "test";
            var fixture = new ListRetrievalWorkerFixture();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", "UserList.xml");
            var data = File.ReadAllText(examplePath);
            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveDocumentAsStringAsync(MalRouteBuilder.UserListUrl(user)))
                .ReturnsAsync(new StringRetrievalWrapper(HttpStatusCode.OK, true, data));

            var sut = fixture.Instance;

            // act
            var result = sut.RetrieveUserListAsync(user).Result;

            // assert
            result.Info.Should().NotBeNull();
            result.Info.UserId.Should().Be("112255");
            result.Info.Username.Should().Be("Test");
            result.Info.Watching.Should().Be(47);
            result.Info.Completed.Should().Be(490);
            result.Info.OnHold.Should().Be(2);
            result.Info.Dropped.Should().Be(23);
            result.Info.PlanToWatch.Should().Be(886);
            result.Info.DaysWatching.Should().Be(93.98);
        }

        #endregion

        private class ListRetrievalWorkerFixture
        {
            #region Constructor

            public ListRetrievalWorkerFixture()
            {
                Instance = new ListRetrievalWorker(PageRetriever);
            }

            #endregion

            #region Properties

            public ListRetrievalWorker Instance { get; }
            public IPageRetriever PageRetriever => PageRetrieverMock.Object;
            public Mock<IPageRetriever> PageRetrieverMock { get; } = new Mock<IPageRetriever>();

            #endregion
        }
    }
}