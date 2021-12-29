using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Extensions;
using WebApi.Filters;
using WebApi.Models;

namespace WebUI.Tests;

using NUnit.Framework;

public class CitizensControllerTests
{
    private static Mock<ICitizenService> _mock;
    private static CitizensController _controller;
    private static List<Citizen> _testCitizens;

    [SetUp]
    public void SetUp()
    {
        _testCitizens = new()
        {
            new (new Guid("07951fc8-024e-4207-af06-33213d7155d1"), "Misha", "Libchenko", 19, CitizenSex.Male),
            new (new Guid("35288b36-3e50-407a-a512-f5164f4be739"), "Stan", "Smith", 27, CitizenSex.Male),
            new (new Guid("c18b57ac-bf6b-4f8b-a3ba-61c6c8dcde2f"), "Jack", "Anderson", 44, CitizenSex.Male),
            new (new Guid("d531e14d-6bfd-444a-b1b2-a22e0888af56"), "Olga", "Popova", 24, CitizenSex.Female),
        };

        _mock = new Mock<ICitizenService>();
        _mock.Setup(service => service.GetCitizensAsync()).ReturnsAsync(_testCitizens);
        _controller = new (_mock.Object);
    }

    [Test]
    public void GetCitizens_ReturnsAResultWithRightListOfCitizens()
    {
        // Act
        Task<OkObjectResult> controllerResponse
            = _controller.GetCitizens(null, 0, 0, 0, 0);

        IndexViewModel? responseValue = controllerResponse.Result.Value as IndexViewModel;

        // Assert
        Assert.IsNotNull(responseValue);
        Assert.IsInstanceOf(typeof(IndexViewModel), responseValue);

        Assert.DoesNotThrow(() => responseValue!.Citizens.ToList());

        var returnedCitizens = responseValue!.Citizens.ToList();
        Assert.IsNotNull(returnedCitizens);

        Assert.AreEqual(_testCitizens.Count, returnedCitizens.Count);
        Assert.AreEqual(_testCitizens.Select(c => c.ToDto()), returnedCitizens);
    }
    
    [TestCase(CitizenSex.Female)]
    [TestCase(CitizenSex.Male)]
    public void GetCitizensBySex_ReturnsResultWithRightListOfCitizens(CitizenSex citizenSexToTest)
    {
        // Act
        Task<OkObjectResult> controllerResponse
            = _controller.GetCitizens(citizenSexToTest, 0, 0, 0, 0);

        IndexViewModel? responseValue = controllerResponse.Result.Value as IndexViewModel;

        // Assert
        Assert.IsNotNull(responseValue);
        Assert.IsInstanceOf(typeof(IndexViewModel), responseValue);

        Assert.DoesNotThrow(() => responseValue!.Citizens.ToList());

        var returnedCitizens = responseValue!.Citizens.ToList();
        Assert.IsNotNull(returnedCitizens);

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
        Task<OkObjectResult> controllerResponse
            = _controller.GetCitizens(null, ageRangeStart, ageRangeEnd, takeAmount, pageNumber);

        IndexViewModel? responseValue = controllerResponse.Result.Value as IndexViewModel;
        List<Citizen> testCitizens = _testCitizens;
        
        AgeRangeFilter ageFilter = new (ageRangeStart, ageRangeEnd);
        PaginationFilter paginationFilter = new (takeAmount, pageNumber);

        // Assert
        Assert.IsNotNull(responseValue);
        Assert.IsInstanceOf(typeof(IndexViewModel), responseValue);

        Assert.DoesNotThrow(() => responseValue!.Citizens.ToList());

        var returnedCitizens = responseValue!.Citizens.ToList();
        Assert.IsNotNull(returnedCitizens);

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
}