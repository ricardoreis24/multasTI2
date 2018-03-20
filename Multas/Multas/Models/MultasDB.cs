using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Multas.Models
{   //class especial que vai extender de uma outra que a framework entity adicionou
    //representa uma base de dados, descreve uma base de dados
    public class MultasDB : DbContext
    {   
        //construtor q indica qual a base de dados a utilizar
        public MultasDB() : base("name=MultasDBConnectionString")
        {

        }
        //descrever nomes das tabelas na base de dados
        //virtual so carrega na memoria se necessita
        public virtual DbSet<Multas> Multas { get; set; } //cria tabela das multas
        public virtual DbSet<Condutores> Condutores { get; set; } //tabela dos condutores
        public virtual DbSet<Agentes> Agentes { get; set; } //tabela dos agentes
        public virtual DbSet<Viaturas> Viaturas { get; set; } //tabela dos agentes
       //não esquecer criar referencia do motor no web.config
       //é necessario referencia ao motor DB
       //Enable-Migrations -EnableAutomaticMigrations -- usar na consola do NuGet para compilar
    }
}