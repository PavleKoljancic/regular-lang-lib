namespace LabTest
{
	public class Token
	{

		public static readonly string op = "operation";
		public static readonly string id = "id";
		public static readonly string lit = "literal";
		public static readonly char ExpresionDeliminater = ';';
		public static readonly string[] operators = { "=", "(", ")", "+", "-", "*", "/","print" };


		public  string type {  get; }
        public  string value {  get; }
		public Token(string type, string value)
		{
			this.type = type == op || type == id || type == lit ? type : throw new Exception("Unknown type!");
			if (type == op && !isOperator(value))
			{
				throw new Exception("Unknown operato!");
            }

			this.value = value;
		}
        public static Boolean isOperator(string s) 
        {
            return operators.Contains(s);
        }
        public Token(string s) 
        {
            if(s=="EOF"||s=="EOE") type=value =s;
            else throw new Exception("Unknown Special!");
        }
	}
}