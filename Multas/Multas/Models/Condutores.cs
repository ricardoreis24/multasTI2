﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Condutores
    {
        //construtor
        public Condutores()
        {
            ListaDeMultas = new HashSet<Multas>();
        }

        [Key]
        public int ID { get; set; } //chave primaria
        //atributos do condutor
        public string Nome { get; set; }
        public string BI { get; set; }
        public string Telemovel { get; set; }
        public DateTime DataNascimento { get; set; }
        //atributos da carta de conduçao
        public string NumCartaConducao { get; set; }
        public string LocalEmissao { get; set; }
        public DateTime DataValidadeCarta { get; set; }
        //referência às multas que um condutor "recebe"
        public virtual ICollection<Multas> ListaDeMultas { get; set; }



    }
}