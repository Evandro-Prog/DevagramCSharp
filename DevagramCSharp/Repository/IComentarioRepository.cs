﻿using DevagramCSharp.Models;

namespace DevagramCSharp.Repository
{
    public interface IComentarioRepository
    {
        public void Comentar(Comentario comentario);
        List<Comentario> GetCOmentarioPorPublicacao(int idPublicacao);
    }
}
