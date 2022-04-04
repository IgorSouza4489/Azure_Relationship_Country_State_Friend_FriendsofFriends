using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureAt.Data;
using AzureAt.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;

namespace AzureAt.Controllers
{
    public class AmigoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AmigoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Amigoes
        public ActionResult Index()
        {
            var amigos = new List<Amigo>();
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
                SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "ConsultarAmigos";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();

                    var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    {
                        while (reader.Read())
                        {
                            var amigo = new Amigo
                            {
                                Id = (int)reader["Id"],

                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                                EstadoOrigem = reader["EstadoOrigem"].ToString(),
                                PaisOrigem = reader["PaisOrigem"].ToString(),
                                Sobrenome = reader["Sobrenome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                Aniversario = (DateTime)reader["Aniversario"]
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
        public ActionResult Create()
        {
            ViewData["EstadoOrigem"] = new SelectList(_context.Estados, "Nome", "Nome");
            ViewData["PaisOrigem"] = new SelectList(_context.Pais, "Nome", "Nome");
            return View();
        }

        // POST: AmigoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>  Create(Amigo amigo)
        {
            //var Foto = UploadBlob(amigo.Imagem).Result;

            var Foto = UploadBlob(amigo.Imagem);
            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";              
                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "CadastrarAmigo";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Nome", amigo.Nome);
                    sqlCommand.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
                    sqlCommand.Parameters.AddWithValue("@Email", amigo.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefone", amigo.Telefone);
                    sqlCommand.Parameters.AddWithValue("@Aniversario", amigo.Aniversario);
                    sqlCommand.Parameters.AddWithValue("@Foto", await Foto);
                    sqlCommand.Parameters.AddWithValue("@EstadoOrigem", amigo.EstadoOrigem);
                    sqlCommand.Parameters.AddWithValue("@PaisOrigem", amigo.PaisOrigem);
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
            return View(amigo);
        }



        public ActionResult Details(int id, Amigo amigo)
        {

            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "ConsultarAmigo";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();

                    var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    {
                        if (reader.Read())
                        {
                            amigo = new Amigo
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                                Sobrenome = reader["Sobrenome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                EstadoOrigem = reader["EstadoOrigem"].ToString(),
                                PaisOrigem = reader["PaisOrigem"].ToString(),
                                Aniversario = (DateTime)reader["Aniversario"]

                            };
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(amigo);
        }

        // GET: Amigoes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AmigoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Amigo amigo)
        {
            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
                var Foto = UploadBlob(amigo.Imagem);

                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "AtualizarAmigo";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlCommand.Parameters.AddWithValue("@Nome", amigo.Nome);
                    sqlCommand.Parameters.AddWithValue("@Sobrenome", amigo.Sobrenome);
                    sqlCommand.Parameters.AddWithValue("@Foto", await Foto);
                    sqlCommand.Parameters.AddWithValue("@Email", amigo.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefone", amigo.Telefone);
                    sqlCommand.Parameters.AddWithValue("@Aniversario", amigo.Aniversario);
                    sqlCommand.Parameters.AddWithValue("@EstadoOrigem", amigo.EstadoOrigem);
                    sqlCommand.Parameters.AddWithValue("@PaisOrigem", amigo.PaisOrigem);

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
            return View(amigo);
        }


        // GET: Amigoes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AmigoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "DeletarAmigo";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);
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
        private bool AmigoExists(int id)
        {
            return _context.Amigo.Any(e => e.Id == id);
        }



        public ActionResult Amiguinhos(int id)
        {
            var amigos = new List<Amiguinhos>();

            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "Listadeeamiguinhos";
                var sqlCommand = new SqlCommand(storedprocedure, connection);

                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();

                    using (var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            var amiguinho = new Amiguinhos
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                                Sobrenome = reader["Sobrenome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                EstadoOrigem = reader["EstadoOrigem"].ToString(),
                                PaisOrigem = reader["PaisOrigem"].ToString(),

                            };
                            amigos.Add(amiguinho);
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


        public async Task<string> UploadBlob(IFormFile imageFile)
        {

            var reader = imageFile.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=igorsouza0489;AccountKey=bvr5caFtow9DNwVRtfPZZetD+WM1XG7dqbG1R/AxscBAkmvT12xoqDRg+T8ZL6DULmBWCd+4U9mS+AStnnryYw==;EndpointSuffix=core.windows.net");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("imagecontainer");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            await blob.UploadFromStreamAsync(reader);
            var uri = blob.Uri.ToString();
            return uri;
        }

    }





}
