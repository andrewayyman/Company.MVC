using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Models
{
	public class Email : BaseEntity
	{
        public string Subject { get; set; }
		public string Body { get; set; }
        public string Reciepints { get; set; }


    }
}
