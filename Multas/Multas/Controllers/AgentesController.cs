using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
            var idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
            //guardar id do novo agente
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
            {
                db.Agentes.Add(agente);
                db.SaveChanges();
                uploadFoto.SaveAs(path);
                return RedirectToAction("Index");
            }

            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            //proteção para o caso de não ter sido fornecido um id
            if (id == null)
            {
                return RedirectToAction("index");
            }
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
        /// 
        /// </summary>
        /// <param name="agentes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")]
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
        /// apresenta na view os dados de um agente com vista à sua eventual eliminação
        /// </summary>
        /// <param name="id"> indentificador do agente </param>
        /// <returns></returns>

        public ActionResult Delete(int? id)
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
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var agentes = db.Agentes.Find(id);
            //remove o agente
            db.Agentes.Remove(agentes);
            //commit
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}