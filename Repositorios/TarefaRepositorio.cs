using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
	public class TarefaRepositorio : ITarefaRepositorio
	{
		private readonly SistemaDeTarefasDBContext _dbContext;
		public TarefaRepositorio(SistemaDeTarefasDBContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<List<TarefaModel>> BuscarTodasTarefas()
		{
			return await _dbContext.Tarefas.Include(x => x.Usuario).ToListAsync();
		}
		public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
		{
			_dbContext.Tarefas.AddAsync(tarefa);
			_dbContext.SaveChangesAsync();
			return tarefa;
		}

		public async Task<bool> Apagar(int id)
		{
			TarefaModel tarefaPorId = await BuscarPorId(id);
			if (tarefaPorId == null)
			{
				throw new Exception($"Tarefa para o ID: {id} não foi encontrada");
			}
			_dbContext.Tarefas.Remove(tarefaPorId);
			_dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
		{
			TarefaModel tarefaPorId = await BuscarPorId(id);
			if(tarefa == null)
			{
				throw new Exception($"Tarefa para o ID: {id} não foi encontrada");
			}
			tarefaPorId.Nome = tarefa.Nome;
			tarefaPorId.Descricao = tarefa.Descricao;
			tarefaPorId.Status = tarefa.Status;
			tarefaPorId.UsuarioId = tarefa.UsuarioId;
			tarefaPorId.Usuario = tarefa.Usuario;

			_dbContext.Tarefas.Update(tarefaPorId);
			_dbContext.SaveChangesAsync();
			return tarefaPorId;
		}

		public async Task<TarefaModel> BuscarPorId(int id)
		{
			return await _dbContext.Tarefas.Include(x => x.Usuario).FirstOrDefaultAsync(x => x.Id == id);
		}

		
	}
}
