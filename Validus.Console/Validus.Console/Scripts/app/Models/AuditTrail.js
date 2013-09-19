var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.AuditTrail = function(id, source, reference, title, description) {
    this.Id = id;
    this.Source = source;
    this.Reference = reference;
    this.Title = title;
    this.Description = description;
};