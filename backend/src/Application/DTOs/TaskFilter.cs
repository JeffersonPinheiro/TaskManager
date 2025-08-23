namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class TaskFilter
    {
        public string? Title { get; set; }
        public Guid? ResponsibleId { get; set; }
        public string? Status { get; set; } // "Pending", "InProgress", "Completed"
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        
        private int? _page;
        public int? Page 
        { 
            get => _page; 
            set => _page = value.HasValue && value.Value > 0 ? value.Value : 1; 
        }
        
        private int? _pageSize;
        public int? PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = value.HasValue && value.Value > 0 ? value.Value : null; 
        }
    }
}
