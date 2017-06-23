﻿using DoctorWebASP.Models;
using DoctorWebASP.ViewModels;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DoctorWebASP.Controllers
{
    public class ReportesController : Controller
    {
        private ApplicationDbContext _context;

        public ReportesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Reportes
        public ActionResult Index()
        {
            string dateString = "02-06-2017";
            DateTime date = DateTime.Parse(dateString);

            var indexViewModel = new ReportesIndexViewModel()
            {
                Personas = getPersonas(date)
            };

            return View(indexViewModel);
        }

        public IEnumerable<Persona> getPersonas(DateTime date)
        {
            var query = from p in _context.Personas
                        where p.FechaCreacion <= DateTime.Now & p.FechaCreacion > date
                        select p;
            return query.ToList();
        }

        public ActionResult getPersonas()
        {
            getPersonasDS ds = new getPersonasDS();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(1200);
            reportViewer.Height = Unit.Percentage(1200);

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


            SqlConnection conx = new SqlConnection(connectionString); SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Personas", conx);

            adp.Fill(ds, "Personas");

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes\R0Personas.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("getPersonasDS", ds.Tables[0]));


            ViewBag.ReportViewer = reportViewer;

            return View();
        }
    }
}