using AzureAt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAt.Controllers
{
    public class AmiguinhosController : Controller
    {
        // GET: AmiguinhosController
        public ActionResult Index()
        {
            var amigos = new List<Amiguinhos>();

            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

            SqlConnection connection = new SqlConnection(connectionString);
            {
                var sp = "ConsultarAmiguinhos";
                var sqlCommand = new SqlCommand(sp, connection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();

                    using (var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            var amigo = new Amiguinhos
                            {
                                Id = (int)reader["Id"],
                                IdAmiguinhos = (int)reader["IdAmiguinhos"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                                Sobrenome = reader["Sobrenome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                EstadoOrigem = reader["EstadoOrigem"].ToString(),
                                PaisOrigem = reader["PaisOrigem"].ToString(),






                            };
                            amigos.Add(amigo);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(amigos);
        }

        // GET: AmiguinhosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AmiguinhosController/Create
        public ActionResult CreateAmiguinho(int Id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAmiguinho(int Id, Amiguinhos amiguinhos)
        {
            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var sp = "CadastrarAmiguinho";
                    var sqlCommand = new SqlCommand(sp, connection);

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@Id", amiguinhos.Id);
                    sqlCommand.Parameters.AddWithValue("@Nome", amiguinhos.Nome);
                    sqlCommand.Parameters.AddWithValue("@Sobrenome", amiguinhos.Sobrenome);
                    sqlCommand.Parameters.AddWithValue("@Email", amiguinhos.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefone", amiguinhos.Telefone);
                    sqlCommand.Parameters.AddWithValue("@Foto", amiguinhos.Foto);
                    sqlCommand.Parameters.AddWithValue("@EstadoOrigem", amiguinhos.EstadoOrigem);
                    sqlCommand.Parameters.AddWithValue("@PaisOrigem", amiguinhos.PaisOrigem);


                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(amiguinhos);
        }

       

        // GET: AmiguinhosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AmiguinhosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AmiguinhosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AmiguinhosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

            SqlConnection connection = new SqlConnection(connectionString);
            {
                var sp = "DeletarAmiguinho";
                var sqlCommand = new SqlCommand(sp, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@AmiguinhosId", id);

                try
                {
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
