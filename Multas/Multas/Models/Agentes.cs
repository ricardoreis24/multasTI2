using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {
        //construtor
        public Agentes()
        {
            ListaDeMultas = new HashSet<Multas>();
        }
        //tudo o que tem [] são complementos
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //descrição dos atributos
        public int ID { get; set; }

        //não esquecer o rebuild
        [Required(ErrorMessage = "Tens de preencher o campo {0} , burro.")]
        //[RegularExpression("[A-ZÍÉÂÁ][a-z]+[a-záéíóúàèìòùâêîôûäëïöüãõç]+ [A-ZÍÉÂÁ][a-z]+[a-záéíóúàèìòùâêîôûäëïöüãõç]+(( | '|-| dos | da | de | e | d')[A-ZÍÉÂÁ][a-z] +[a-záéíóúàèìòùâêîôûäëïöüãõç]+[A-ZÍÉÂÁ][a-z]+[a-záéíóúàèìòùâêîôûäëïöüãõç]+){1, 3}", ErrorMessage = "O {0} apenas pode conter letras e espaços em branco. Cada palavra começa em Maiúscula seguida de minúscula")]

        public string Nome { get; set; }
        //[RegularExpression("[A-ZÍÉÂÁ]*[a-záéíóúàèìòùâêîôûäëïöüãõç ]*", ErrorMessage = "A {0} não é válida")]
        [Required(ErrorMessage = "Tens de preencher o campo {0} , burro.")]
        public string Esquadra { get; set; }
        public string Fotografia { get; set; }
        //referência às multas que um agente "emite"
        public virtual ICollection<Multas> ListaDeMultas { get; set; }


    }
}