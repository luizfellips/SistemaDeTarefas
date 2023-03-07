using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
	public class UsuarioRepositorio : IUsuarioRepositorio
	{
		private readonly SistemaDeTarefasDBContext _dbContext;
		public UsuarioRepositorio(SistemaDeTarefasDBContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<List<UsuarioModel>> BuscarTodosOsUsuarios()
		{
			return await _dbContext.Usuarios.ToListAsync();
		}
		public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
		{
			_dbContext.Usuarios.AddAsync(usuario);
			_dbContext.SaveChangesAsync();
			return usuario;
		}

		public async Task<bool> Apagar(int id)
		{
			UsuarioModel usuarioPorId = await BuscarPorId(id);
			if (usuarioPorId == null)
			{
				throw new Exception($"Usuário para o ID: {id} não foi encontrado");
			}
			_dbContext.Usuarios.Remove(usuarioPorId);
			_dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
		{
			UsuarioModel usuarioPorId = await BuscarPorId(id);
			if(usuarioPorId == null)
			{
				throw new Exception($"Usuário para o ID: {id} não foi encontrado");
			}
			usuarioPorId.Nome = usuario.Nome;
			usuarioPorId.Email = usuario.Email;
			_dbContext.Usuarios.Update(usuarioPorId);
			_dbContext.SaveChangesAsync();
			return usuarioPorId;
		}

		public async Task<UsuarioModel> BuscarPorId(int id)
		{
			return await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
		}

		
	}
}
