namespace ApplicationCore.Domain.EN
{
    public class Mensaje
    {
        public virtual long IdMensaje { get; set; }
        public virtual string Contenido { get; set; } = string.Empty;
        public virtual System.DateTime FechaEnvio { get; set; }

        // FKs
        public virtual long UsuarioId { get; set; }
        public virtual long AnuncioId { get; set; }

        // Navigation
        public virtual Usuario? Usuario { get; set; }
        public virtual Anuncio? Anuncio { get; set; }
    }
}
