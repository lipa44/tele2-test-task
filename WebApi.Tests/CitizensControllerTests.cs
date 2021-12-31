namespace WebUI.Tests;

using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Dto;
using WebApi.Extensions;
using WebApi.Filters;
using WebApi.Models;
using NUnit.Framework;

public class CitizensControllerTests
{
    private static Mock<ICitizenService> _mock;
    private static CitizensController _controller;
    private static List<Citizen> _testCitizens;
    private static List<Guid> _citizensId;

    [OneTimeSetUp]
    public void OneTimeCitizenIdsSetUp()
    {
        _citizensId = new ()
        {
            new ("07951fc8-024e-4207-af06-33213d7155d1"),
            new ("35288b36-3e50-407a-a512-f5164f4be739"),
            new ("c18b57ac-bf6b-4f8b-a3ba-61c6c8dcde2f"),
            new ("d531e14d-6bfd-444a-b1b2-a22e0888af56"),
        };
    }

    [SetUp]
    public void ControllerSetUp()
    {
        _mock = new Mock<ICitizenService>();

        _testCitizens = new ()
        {
            new (_citizensId[0], "Misha", "Libchenko", 19, CitizenSex.Male),
            new (_citizensId[1], "Stan", "Smith", 27, CitizenSex.Male),
            new (_citizensId[2], "Jack", "Anderson", 44, CitizenSex.Male),
            new (_citizensId[3], "Olga", "Popova", 24, CitizenSex.Female),
        };

        _mock.Setup(service => service.GetCitizensAsync()).ReturnsAsync(_testCitizens);

        _testCitizens.ForEach(citizen =>
            _mock.Setup(service => service.FindCitizenByIdAsync(citizen.Id)).ReturnsAsync(citizen));

        _controller = new (_mock.Object);
    }

    [Test]
    public void GetCitizens_ReturnsAResultWithRightListOfCitizens()
    {
        // Act
        var controllerResponse
            = _controller.GetCitizens(null, 0, 0, 0, 0);

        // Assert
        Assert.IsNotNull(controllerResponse);
        Assert.IsNotNull(controllerResponse.Result);
        var responseActionResult = controllerResponse.Result;

        Assert.IsAssignableFrom(typeof(OkObjectResult), responseActionResult.Result);
        var responseObjectResult = responseActionResult.Result as OkObjectResult;

        Assert.IsAssignableFrom(typeof(IndexViewModel), responseObjectResult?.Value);
        Assert.AreEqual(200, responseObjectResult?.StatusCode);
        var responseViewModel = responseObjectResult?.Value as IndexViewModel;

        Assert.IsNotNull(responseViewModel?.Citizens);
        var returnedCitizens = responseViewModel!.Citizens.ToList();

        Assert.AreEqual(_testCitizens.Count, returnedCitizens.Count);
        Assert.AreEqual(_testCitizens.Select(c => c.ToDto()), returnedCitizens);
    }
    
    [TestCase(CitizenSex.Female)]
    [TestCase(CitizenSex.Male)]
    public void GetCitizensBySex_ReturnsResultWithRightListOfCitizens(CitizenSex citizenSexToTest)
    {
        // Act
        var controllerResponse
            = _controller.GetCitizens(citizenSexToTest, 0, 0, 0, 0);

        // Assert
        Assert.IsNotNull(controllerResponse);
        Assert.IsNotNull(controllerResponse.Result);
        var responseActionResult = controllerResponse.Result;

        Assert.IsAssignableFrom(typeof(OkObjectResult), responseActionResult.Result);
        var responseObjectResult = responseActionResult.Result as OkObjectResult;

        Assert.IsAssignableFrom(typeof(IndexViewModel), responseObjectResult?.Value);
        Assert.AreEqual(200, responseObjectResult?.StatusCode);
        var responseViewModel = responseObjectResult?.Value as IndexViewModel;

        Assert.IsNotNull(responseViewModel?.Citizens);
        var returnedCitizens = responseViewModel!.Citizens.ToList();

        var citizensBySexCount = returnedCitizens.Count(c => c.Sex == citizenSexToTest.ToString());
        Assert.AreEqual(citizensBySexCount, returnedCitizens.Count);

        var resultAfterSexFilter = _testCitizens
            .Where(c => c.Sex == citizenSexToTest)
            .Select(c => c.ToDto()).ToList();

        Assert.AreEqual(resultAfterSexFilter, returnedCitizens);
    }
    
    [TestCase(0, 0, 0, 0)]
    [TestCase(10, 15, 10, 1)]
    [TestCase(18, 21, 10, 1)]
    [TestCase(18, 50, 10, 1)]
    [TestCase(25, 45, 1, 2)]
    [TestCase(25, 45, 2, 2)]
    public void GetCitizensWithPagination_ReturnsResultWithRightListOfCitizens(
        int ageRangeStart,
        int ageRangeEnd,
        int takeAmount,
        int pageNumber)
    {
        // Act
        var controllerResponse
            = _controller.GetCitizens(null, ageRangeStart, ageRangeEnd, takeAmount, pageNumber);
        
        var testCitizens = _testCitizens.Select(c => c).ToList();
        AgeRangeFilter ageFilter = new (ageRangeStart, ageRangeEnd);
        PaginationFilter paginationFilter = new (takeAmount, pageNumber);

        // Assert
        Assert.IsNotNull(controllerResponse);
        Assert.IsNotNull(controllerResponse.Result);
        var responseActionResult = controllerResponse.Result;

        Assert.IsAssignableFrom(typeof(OkObjectResult), responseActionResult.Result);
        var responseObjectResult = responseActionResult.Result as OkObjectResult;

        Assert.IsAssignableFrom(typeof(IndexViewModel), responseObjectResult?.Value);
        Assert.AreEqual(200, responseObjectResult?.StatusCode);
        var responseViewModel = responseObjectResult?.Value as IndexViewModel;

        Assert.IsNotNull(responseViewModel?.Citizens);
        var returnedCitizens = responseViewModel!.Citizens.ToList();

        var deletedByAgeCitizens = testCitizens
            .Where(c => c.Age < ageFilter.Start || c.Age > ageFilter.End).ToList();

        testCitizens.RemoveAll(c => deletedByAgeCitizens.Contains(c));
        
        var deletedByPaginationCitizens = testCitizens
            .Take((paginationFilter.Page - 1) * paginationFilter.Take).ToList();

        var expectedCitizens = testCitizens
            .Except(deletedByPaginationCitizens)
            .Select(c => c.ToDto()).ToList();

        Assert.AreEqual(expectedCitizens.Count, returnedCitizens.Count);
        Assert.AreEqual(expectedCitizens, returnedCitizens);
    }

    [Test]
    public void GetCitizensByExistingId_EachCitizenFound()
    {
        // Act
        var controllerResponses = 
            _citizensId.Select(id => _controller.GetCitizenById(id)).ToList();
        
        controllerResponses.ForEach(controllerResponse =>
        {
            Assert.IsNotNull(controllerResponse);
            Assert.IsNotNull(controllerResponse.Result);
            var responseActionResult = controllerResponse.Result;

            Assert.IsAssignableFrom(typeof(OkObjectResult), responseActionResult.Result);
            var responseObjectResult = responseActionResult.Result as OkObjectResult;

            Assert.IsAssignableFrom(typeof(CitizenFullDto), responseObjectResult?.Value);
            Assert.AreEqual(200, responseObjectResult?.StatusCode);
            var responseCitizenFullDto = responseObjectResult!.Value as CitizenFullDto;
            
            var testCitizensFullDto = _testCitizens.Select(c => c.ToFullDto());
            Assert.IsTrue(testCitizensFullDto.Any(c => c == responseCitizenFullDto));
        });
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(10)]
    public void GetCitizensByNotExistingId_EachCitizenNotFound(int amountOfIds)
    {
        var notExistingIds = new List<Guid>();
        for (var i = 0; i < amountOfIds; ++i) notExistingIds.Add(Guid.NewGuid());

        // Act
        var controllerResponses = 
            notExistingIds.Select(id => _controller.GetCitizenById(id)).ToList();

        controllerResponses.ForEach(controllerResponse =>
        {
            Assert.IsNotNull(controllerResponse);
            Assert.IsNotNull(controllerResponse.Result);
            var responseActionResult = controllerResponse.Result;

            Assert.IsAssignableFrom(typeof(NotFoundResult), responseActionResult.Result);
            var responseResult = responseActionResult.Result as NotFoundResult;

            Assert.AreEqual(404, responseResult!.StatusCode);
        });
    }
}