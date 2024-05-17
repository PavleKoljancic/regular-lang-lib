using LabTest;

namespace LabTest
{

	class Program 
	{
		public static void Main(string [] args) 
		{	
			if(args.Length<1) throw new ArgumentException("No File name provided!");

			string input  = File.ReadAllText(args[0]);
			Lexer lexer = new Lexer(input);
			
		
		}	
	}

}