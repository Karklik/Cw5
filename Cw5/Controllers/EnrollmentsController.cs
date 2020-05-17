﻿using Cw5.DAL;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idEnrollment}")]
        public IActionResult GetEnrollment(int idEnrollment)
        {
            var enrollment = _dbService.GetEnrollment(idEnrollment);
            if (enrollment != null)
                return Ok(new GetEntrollmentResponse
                {
                    IdEnrollment = enrollment.IdEnrollment,
                    IdStudy = enrollment.IdStudy,
                    Semester = enrollment.Semester,
                    StartDate = enrollment.StartDate
                });
            else
                return NotFound("Enrollment not found");
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            try
            {
                DateTime.ParseExact(request.BirthDate, "dd.MM.yyyy", null);

                var enrollment = _dbService.CreateStudentEnrollment(request);
                if (enrollment != null)
                {
                    return CreatedAtAction(nameof(GetEnrollment), new { idEnrollment = enrollment.IdEnrollment }, new GetEntrollmentResponse
                    {
                        IdEnrollment = enrollment.IdEnrollment,
                        IdStudy = enrollment.IdStudy,
                        Semester = enrollment.Semester,
                        StartDate = enrollment.StartDate
                    });
                }
                else
                    return StatusCode(500, "Failed to process request");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("promotions")]
        public IActionResult SemesterPromote(SemestrPromoteRequest request)
        {
            var studies = _dbService.GetStudies(request.Studies);
            if (studies == null)
                return NotFound("Studies dosen't exits");
            var enrollment = _dbService.GetEnrollment(studies.IdStudy, request.Semester);
            if (enrollment == null)
                return NotFound("Enrollment for semester dosen't exits");

            var newEnrollment = _dbService.SemesterPromote(studies.IdStudy, request.Semester);
            return CreatedAtAction(nameof(GetEnrollment),
                new { idEnrollment = enrollment.IdEnrollment },
                new GetEntrollmentResponse
                {
                    IdEnrollment = newEnrollment.IdEnrollment,
                    IdStudy = newEnrollment.IdStudy,
                    Semester = newEnrollment.Semester,
                    StartDate = newEnrollment.StartDate
                });
        }
    }
}