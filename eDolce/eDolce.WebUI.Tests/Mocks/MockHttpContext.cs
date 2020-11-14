using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eDolce.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest request;
        private MockRespose respose;
        private HttpCookieCollection cookies;
        private IPrincipal FakeUser;
        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.respose = new MockRespose(cookies);
        }

        public override IPrincipal User {
            get { return this.FakeUser; }
            set {this.FakeUser = value; }
        } 
    }

    public class MockRespose : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;
        public MockRespose(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }
        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }
    }

    public class MockRequest:HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;
        public MockRequest(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }
        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }

    }
}
