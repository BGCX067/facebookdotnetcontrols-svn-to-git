using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook.Web;
using Facebook.Service;

namespace FacebookControls
{
    /// <summary>
    /// Creates a "baseball card" of a user, using their
    /// square profile picture, their name, and a link to their
    /// profile. Uses the ViewState for caching.
    /// </summary>
    [ToolboxData("<{0}:userCard runat=server></{0}:userCard>")]
    public class userCard : WebControl
    {
        private Facebook.Web.FacebookApplication _fbService;
        /// <summary>
        /// Property to be set by the caller running the Facebook Application
        /// control instance
        /// </summary>
        [Bindable(true)]
        [Category("Service Handler")]
        [Localizable(false)]
        public Facebook.Web.FacebookApplication fbApp
        {
            get { return this._fbService; }
            set { this._fbService = value; }
        }

        /// <summary>
        /// The userID # for the user whose profile we're downloading
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        public string UserID
        {
            get
            {
                object o = ViewState["UserID"];
                if (o == null)
                {
                    return string.Empty; //Critical error
                }
                else
                    return (string)o;
            }
            set { ViewState["UserID"] = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        public string UserName
        {
            get
            {
                object o = ViewState["UserName"];
                if (o == null)
                {
                    loadProfileValuesFromFB(); //Make the call to Facebook
                    return string.Empty; /*Return an empty string; we don't
                    want to keep looping through querying facebook if there's
                    an error with our query so just bail out and let force the
                    caller to retry. */

                }
                else
                    return (string)o;
            }
            set { ViewState["UserName"] = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        public string ProfileImageURL
        {
            get {
                object o = ViewState["ProfileImageURL"];
                if (o == null)
                {
                    loadProfileValuesFromFB(); //Make the call to Facebook
                    return string.Empty; /*Return an empty string; we don't
                    want to keep looping through querying facebook if there's
                    an error with our query so just bail out and let force the
                    caller to retry. */
                  
                }
                else
                    return (string)o;
            }
            set { ViewState["ProfileImageURL"] = value; }
        }

        /// <summary>
        /// Used at the end of the sentence below the person's image
        /// it can say "[USER NAME]'s Profile" or whatever. Anything
        /// that trails the user name is used here
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        [DefaultValue("'s Profile")]
        public string ProfileDescriptor
        {
            get
            {
                object o = ViewState["ProfileDescriptor"];
                if (o == null)
                {
                    loadProfileValuesFromFB(); //Make the call to Facebook
                    return string.Empty; /*Return an empty string; we don't
                    want to keep looping through querying facebook if there's
                    an error with our query so just bail out and let force the
                    caller to retry. */

                }
                else
                    return (string)o;
            }
            set { ViewState["ProfileDescriptor"] = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        public string ProfileURL
        {
            get
            {
                object o = ViewState["ProfileURL"];
                if (o == null)
                {
                    loadProfileValuesFromFB(); //Make the call to Facebook
                    return string.Empty; /*Return an empty string; we don't
                    want to keep looping through querying facebook if there's
                    an error with our query so just bail out and let force the
                    caller to retry. */

                }
                else
                    return (string)o;
            }
            set { ViewState["ProfileURL"] = value; }
        }

        private string _profileItemsString;
        /// <summary>
        /// This is the string used to determine which items to
        /// grab on the query to Facebook. By default we grab just
        /// the 
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        [DefaultValue("Name,SquarePictureUrl")]
        public string ProfileItemsString
        {
            get { return this._profileItemsString; }
            set { this._profileItemsString = value; }
        }

        public userCard() : base() {
            initialize();
        }

        public userCard(HtmlTextWriterTag tag)
            : base(tag)
        {
            initialize();
        }

        public userCard(string tag)
            : base(tag)
        {
            initialize();
        }

        protected void initialize()
        {
            this.ProfileItemsString = "Name,SquarePictureUrl"; //Default value
        }

        public void loadProfileValuesFromFB()
        {
            if (this.fbApp == null)
                throw new ApplicationException(Resources.ErrorMessages.FBServiceNotEstablished);
            User fbUser;
            fbUser = this.fbApp.Service.Users.GetUser(this.UserID, null);
            if(fbUser == null){
                //Raise an error if we can't download the profile information for this user
                throw new ApplicationException(Resources.ErrorMessages.UserServiceCouldNotConnect + this.UserID);
            }
            this.UserName = fbUser.Name;
            this.ProfileImageURL = fbUser.SquarePictureUrl;
            this.UserID = fbUser.ID; //Not necessary, but not expensive either.
            this.ProfileURL = Resources.Strings.fbBaseProfileURL + this.UserID;
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.WriteBeginTag("div");
            output.WriteStyleAttribute("width", "350px");
            output.Write(HtmlTextWriter.TagRightChar);            
            output.WriteBeginTag("div");
            output.WriteBeginTag("a");
            output.WriteAttribute("href", this.ProfileURL);
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteBeginTag("img");
            output.WriteAttribute("src", this.ProfileImageURL);
            output.WriteAttribute("alt", this.UserName);
            output.Write(HtmlTextWriter.SelfClosingTagEnd);
            output.WriteEndTag("a");
            output.WriteEndTag("div");
            output.WriteBeginTag("div");
            output.WriteStyleAttribute("float", "right");
            output.WriteStyleAttribute("vertical-align", "top");
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteBeginTag("a");
            output.WriteAttribute("href", this.ProfileURL);
            output.WriteStyleAttribute("font-weight", "bold");
            output.Write(HtmlTextWriter.TagRightChar);
            output.Write(this.UserName);
            output.WriteEndTag("a");
            output.Write(this.ProfileDescriptor);
            output.WriteEndTag("div");
            output.WriteEndTag("div");
        }
    }
}
