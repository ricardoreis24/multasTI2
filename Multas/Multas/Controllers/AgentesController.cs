using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        private readonly MultasDB db = new MultasDB();

        // GET: Agentes
        public ActionResult Index()
        {
            //db.Agentes.ToList() em SQL SELECT * FROM Agentes;
            //enviar para a view uma lista com todos os agente da base de dados
            //em sql: select * from agentes order by nome;
            var listaDeAgentes = db.Agentes.ToList().OrderBy(a => a.Nome);

            return View(listaDeAgentes);
        }

        // GET: Agentes/Details/5
        //quando tem ? significa que pode ser preenchimento facultativo
        public ActionResult Details(int? id)
        {
            //proteção para o caso de não ter sido fornecido um id
            if (id == null) return RedirectToAction("index");
            //procura na BD, o agente cujo ID foi fornecido

            var agente = db.Agentes.Find(id);
            //proteção para o caso de não ter sido encontrado qualquer agente
            //que tenha o ID fornecido
            if (agente == null) return RedirectToAction("index");
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            //apresenta a view para se inserir num agente
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Esquadra")] Agentes agente, HttpPostedFileBase uploadFoto)
        {
            //escrever um ficheiro com a fotografia no disco na pasta 'imagens'          
            //especificar o id do agente
            //formatar o tamanho da imagem
            //guardar id do novo agente

            //testar se há registo na tabela dos agentes
            //if (db.Agentes.Count() != 0) { }

            //ou então usar a instrução TRY/CATCH
            var idNovoAgente = 0;
            try
            {
                idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
            }
            catch (Exception e)
            {
                idNovoAgente = 1;
            }

            //guardar o id do novo agente
            agente.ID = idNovoAgente;
            //especificar (escolher) o nome do ficheiro
            var nomeImagem = "Agente_" + idNovoAgente + ".jpg";

            //var auxiliar
            var path = "";

            //validar se imagem foi fornecida
            if (uploadFoto != null)
            {
                //o ficheiro foi fornecido
                //criar o caminho
                path = Path.Combine(Server.MapPath("~/imagens"), nomeImagem);

                //guardar o nome do ficheiro na bd
                agente.Fotografia = nomeImagem;
            }
            else
            {
                //não foi fornecido qualquer ficheiro
                ModelState.AddModelError("", "Não foi forncecida uma imagem...");
                //devolver o controlo da view
                return View(agente);
            }

            //escreve os dados de um novo agente na DB
            //ModelState.IsValid confronta os dados fornecidos na view
            if (ModelState.IsValid)
                try
                {
                    //adiciona novo agente
                    db.Agentes.Add(agente);
                    //faz commit
                    db.SaveChanges();
                    //escrever o ficheiro da foto no disco
                    uploadFoto.SaveAs(path);
                    //returna pagina index
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Houve um erro com a criação de um novo agente");
                }

            // devolve os dados do Agente `a View
            return View(agente);
        }

        // GET: Agentes/Edit/5
        private ActionResult Edit(int? id)
        {
            //proteção para o caso de não ter sido fornecido um id
            if (id == null) return RedirectToAction("index");
            //procura na BD, o agente cujo ID foi fornecido

            var agente = db.Agentes.Find(id);
            //proteção para o caso de não ter sido encontrado qualquer agente
            //que tenha o ID fornecido
            if (agente == null) return RedirectToAction("index");
            return View(agente);
        }


        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// </summary>
        /// <param name="agentes"></param>
        /// <returns></returns>
        [
            HttpPost]
        [ValidateAntiForgeryToken]
        private ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")]
            Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                //neste caso já existe agente
                //apenas quero editar os seus dado
                db.Entry(agentes).State = EntityState.Modified;
                //faz o commit
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agentes);
        }

        // GET: Agentes/Delete/5
        /// <summary>
        ///     apresenta na view os dados de um agente com vista à sua eventual eliminação
        /// </summary>
        /// <param name="id"> indentificador do agente </param>
        /// <returns></returns>
        private ActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            //pesquisar pelo agente cujo ID foi fornecido
            var agentes = db.Agentes.Find(id);
            //verificar se o agente foi encontrado
            if (agentes == null)
            {
                return RedirectToAction("Index");
            }

            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [
            HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        private ActionResult DeleteConfirmed(int id)
        {
            var agente = db.Agentes.Find(id);
            try
            {
            
            //remove o agente
            db.Agentes.Remove(agente);
            //commit
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", string.Format("Houve um erro com a eliminação do agente nº {0} - {1}", id, agente.Nome));
                
            }
            // se cheguei aqui é porque houve um problema
            return View(agente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}