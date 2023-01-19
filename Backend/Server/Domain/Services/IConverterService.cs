namespace Domain.Services;

public interface IConverterService
{ 
    Stream ToPdf(string text);
}