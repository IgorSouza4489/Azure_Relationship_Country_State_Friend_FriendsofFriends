using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureAt.Data;
using AzureAt.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace AzureAt.Controllers
{
    public class EstadoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var estados = new List<Estado>();
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "ConsultarEstados";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    {
                        while (reader.Read())
                        {
                            var estado = new Estado
                            {
                                EstadoId = (int)reader["EstadoId"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                                PaisOrigem = reader["PaisOrigem"].ToString(),                      
                            };
                            estados.Add(estado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(estados);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estado = await _context.Estados
                .Include(e => e.Pais)
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estado == null)
            {
                return NotFound();
            }

            return View(estado);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estado estado)
        {
            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "CadastrarEstado";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Nome", estado.Nome);
                    sqlCommand.Parameters.AddWithValue("@Foto", estado.Foto);
                    sqlCommand.Parameters.AddWithValue("@PaisOrigem", estado.PaisOrigem);
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(estado);
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Nome,Foto,PaisOrigem")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "AtualizarEstado";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@EstadoId", id);
                    sqlCommand.Parameters.AddWithValue("@Nome", estado.Nome);
                    sqlCommand.Parameters.AddWithValue("@Foto", estado.Foto);
                    sqlCommand.Parameters.AddWithValue("@PaisOrigem", estado.PaisOrigem);

                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(estado);
        }

      
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "DeletarEstado";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@EstadoId", id);
                try
                {
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoExists(int id)
        {
            return _context.Estados.Any(e => e.EstadoId == id);
        }
    }
}
