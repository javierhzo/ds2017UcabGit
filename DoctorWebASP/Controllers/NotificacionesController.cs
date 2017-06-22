﻿using DoctorWebASP.Controllers.Helpers;
using DoctorWebASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorWebASP.Controllers
{
    public class NotificacionesController : Controller
    {
        // GET: Notificaciones
        public ActionResult Index(string nombre = null, int indice = 0, int filas = 5)
        {
            var cantidadPaginas = 0;
            var notificaciones = Notificacion.ObtenerTodos(out cantidadPaginas, nombre, indice, filas);
            ViewData["nombre"] = nombre;
            ViewData["filas"] = filas;
            ViewData["permitirSiguiente"] = indice < cantidadPaginas-1;
            ViewData["siguienteIndice"] = indice + 1;
            ViewData["permitirAnterior"] = indice > 0;
            ViewData["anteriorIndice"] = indice - 1;
            return View(model: notificaciones);
        }

        public ActionResult Detail(int codigo)
        {
            Notificacion model = null;
            if (codigo != 0)
            {
                model = Notificacion.Obtener(codigo);
            }
            else
            {
                model = new Notificacion();
            }
            if (model != null)
            {
                this.RellenarCombos(model);
                return View(model: model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var model = new Notificacion();
            try
            {
                model.NotificacionId = 0;
                model.Actualizar(collection);
                var mensaje = String.Empty;
                var sinProblemas = Notificacion.Guardar(model, out mensaje);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("Error", exception.Message);
                this.RellenarCombos(model);
                return View("Detail", model);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(FormCollection collection)
        {
            var model = new Notificacion()
            {
                //NotificacionId = codigo
            };
            try
            {
                model.Actualizar(collection);
                var mensaje = String.Empty;
                var sinProblemas = Notificacion.Guardar(model, out mensaje);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                if (model == null)
                    return RedirectToAction("Index");

                ModelState.AddModelError("Error", exception.Message);

                this.RellenarCombos(model);
                return View("Detail", model);
            }
        }

        public ActionResult Delete(int codigo)
        {
            var model = Notificacion.Obtener(codigo);
            if (model != null)
                return View(model: model);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            try
            {
                var codigo = int.Parse(collection["NotificacionId"]);
                var mensaje = String.Empty;
                var sinProblemas = Notificacion.Borrar(codigo, out mensaje);
                
            }
            catch
            {
                
            }
            return RedirectToAction("Index");
        }
    }
}