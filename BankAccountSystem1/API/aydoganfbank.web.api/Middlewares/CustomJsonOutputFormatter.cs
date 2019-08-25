using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.Middlewares
{
    public class CustomJsonOutputFormatter : JsonOutputFormatter
    {
        public CustomJsonOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool) 
            : base(serializerSettings, charPool)
        {
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (selectedEncoding == null)
            {
                throw new ArgumentNullException(nameof(selectedEncoding));
            }

            using (TextWriter writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                WriteObject(writer, new
                {
                    result = context.Object,
                    resultCode = context.HttpContext.Items["resultCode"],
                    resultMessage = context.HttpContext.Items["resultMessage"]
                });

                await writer.FlushAsync();
            }
        }
    }
}
