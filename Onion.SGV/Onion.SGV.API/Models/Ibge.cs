namespace Onion.SGV.API.Models
{
    public class Regiao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
    }
    public class UF
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public Regiao Regiao { get; set; }
    }
    public class Mesorregiao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public UF Uf { get; set; }
    }
    public class Microrregiao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Mesorregiao Mesorregiao { get; set; }
    }
    public class Ibge
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Microrregiao Microrregiao { get; set; }
    }
}
