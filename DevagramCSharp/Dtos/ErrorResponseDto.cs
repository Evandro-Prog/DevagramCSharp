namespace DevagramCSharp.Dtos
{
    public class ErrorResponseDto
    {
        public int Status {  get; set; }
        public string Descricao { get; set; }

        public List<string> Erros {  get; set; }
    }
}
