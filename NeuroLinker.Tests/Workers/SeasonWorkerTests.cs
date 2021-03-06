﻿using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using NeuroLinker.Enumerations;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Workers;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using NeuroLinker.ResponseWrappers;

namespace NeuroLinker.Tests.Workers
{
    public class SeasonWorkerTests
    {
        #region Public Methods

        [Test]
        public void RetrievingCurrentSeasonWorksCorrectly()
        {
            // arrange
            var fixture = new SeasonWorkerFixture();
            var sut = fixture.Instance;

            // act
            var result = sut.RetrieveCurrentSeason().Result;

            // assert
            result.ResponseData.SeasonShows.Count.Should().Be(285);
        }

        [Test]
        public void RetrievingSpecificSeasonWorksCorrectly()
        {
            // arrange
            const int year = 2017;
            const Seasons season = Seasons.Spring;
            var fixture = new SeasonWorkerFixture();
            var sut = fixture.Instance;

            // act
            var resultWrapper = sut.GetSeasonData(year, season).Result;

            // assert
            var result = resultWrapper.ResponseData.SeasonShows;
            result.Count.Should().Be(95);
            result.Should().Contain(x => x.Id == 25777);
            result.Should().Contain(x => x.Title == "Shingeki no Kyojin Season 2");
            result.Should().Contain(x => x.Id == 34733);
            result.Should().Contain(x => x.Title == "One Room Special");
        }

        #endregion

        private class SeasonWorkerFixture
        {
            #region Constructor

            public SeasonWorkerFixture()
            {
                var seasonDoc = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", "2017Spring.html");
                using (var htmlFile = File.Open(examplePath, FileMode.Open))
                {
                    seasonDoc.Load(htmlFile);
                }

                PageRetrieverMock
                    .Setup(t => t.RetrieveHtmlPageAsync(It.IsAny<string>()))
                    .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.OK, true, seasonDoc));

                Instance = new SeasonWorker(PageRetriever);
            }

            #endregion

            #region Properties

            public SeasonWorker Instance { get; }

            public IPageRetriever PageRetriever => PageRetrieverMock.Object;
            public Mock<IPageRetriever> PageRetrieverMock { get; } = new Mock<IPageRetriever>();

            #endregion
        }
    }
}