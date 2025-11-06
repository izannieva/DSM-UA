using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class UsuarioCEN
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IUnitOfWork _uow;

        public UsuarioCEN(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
        {
            _usuarioRepo = usuarioRepo;
            _uow = uow;
        }

        // CRUD Operations

        // Create - Crear recibe sólo atributos obligatorios según domain.model.json
        public long Crear(string nombre, string email, string password, string? telefono = null)
        {
            var u = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Password = password,
                Telefono = telefono
            };

            _usuarioRepo.New(u);
            _uow.SaveChanges();
            return u.IdUsuario;
        }

        // Read - Consultar por ID
        public Usuario? ReadOID(long id)
        {
            return _usuarioRepo.DamePorOID(id);
        }

        // Read - Consultar todos
        public IEnumerable<Usuario> ReadAll()
        {
            return _usuarioRepo.DameTodos();
        }

        // Update - Modificar todos los campos
        public void Modify(long id, string nombre, string email, string password, string? telefono = null)
        {
            var usuario = _usuarioRepo.DamePorOID(id);
            if (usuario == null)
                throw new System.Exception("Usuario no encontrado");

            usuario.Nombre = nombre;
            usuario.Email = email;
            usuario.Password = password;
            usuario.Telefono = telefono;

            _usuarioRepo.Modify(usuario);
            _uow.SaveChanges();
        }

        // Delete - Eliminar
        public void Destroy(long id)
        {
            var usuario = _usuarioRepo.DamePorOID(id);
            if (usuario == null)
                throw new System.Exception("Usuario no encontrado");

            _usuarioRepo.Destroy(usuario);
            _uow.SaveChanges();
        }

        // Authentication Operations

        /// <summary>
        /// Método de login que valida las credenciales del usuario
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Usuario autenticado o null si las credenciales son inválidas</returns>
        public Usuario? Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new System.ArgumentException("El email y la contraseña son obligatorios");

            // Obtener todos los usuarios y buscar por email
            var usuarios = _usuarioRepo.DameTodos();
            var usuario = usuarios.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (usuario == null)
                return null; // Usuario no encontrado

            // Verificar la contraseña
            if (usuario.Password != password) // En una implementación real, usar hash de contraseña
                return null; // Contraseña incorrecta

            return usuario; // Login exitoso
        }

        // Custom Operations

        // Cambiar estado de anuncio
        public void CambiarEstado(long anuncioId, string nuevoEstado)
        {
            var anuncio = _usuarioRepo.DamePorOID(anuncioId)?.Anuncios?.FirstOrDefault(a => a.IdAnuncio == anuncioId);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            anuncio.Estado = nuevoEstado;
            _uow.SaveChanges();
        }

        // Cambiar título de anuncio
        public void CambiarTitulo(long anuncioId, string nuevoTitulo)
        {
            var anuncio = _usuarioRepo.DamePorOID(anuncioId)?.Anuncios?.FirstOrDefault(a => a.IdAnuncio == anuncioId);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            anuncio.Titulo = nuevoTitulo;
            _uow.SaveChanges();
        }

        // Destacar anuncio
        public void DestacarAnuncio(long anuncioId)
        {
            var anuncio = _usuarioRepo.DamePorOID(anuncioId)?.Anuncios?.FirstOrDefault(a => a.IdAnuncio == anuncioId);
            if (anuncio == null)
                throw new System.Exception("Anuncio no encontrado");

            // Implementar lógica específica para destacar el anuncio
            anuncio.Estado = "Destacado";
            // Aquí se podrían añadir más campos o lógica específica para anuncios destacados
            _uow.SaveChanges();
        }
    }
}
