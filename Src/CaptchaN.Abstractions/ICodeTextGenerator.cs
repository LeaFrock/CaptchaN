namespace CaptchaN.Abstractions;

public interface ICodeTextGenerator
{
    string Generate(int length);
}