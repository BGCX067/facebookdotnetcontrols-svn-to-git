using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacebookControls
{
    /// <summary>
    /// Renders a localized, relative time differential; resolves the problem of accounting
    /// for time zone changes.
    /// </summary>
    [ToolboxData("<{0}:relativeTimeDifference runat=server></{0}:relativeTimeDifference>")]
    public class relativeTimeDifference : WebControl
    {
        //Private data member storing the start time value
        private System.DateTime startTime_;

        /// <summary>
        /// Start time of the timespan we want to output. Default value
        /// is the current time.
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        public System.DateTime StartTime
        {
            get
            {
                return this.startTime_;
            }

            set
            {
               this.startTime_ = value;
            }
        }

        //Private data member for storing end time values
        private System.DateTime endTime_;

        /// <summary>
        /// End time of the timespan that we want to output. By
        /// default this timespan is 0, as the default values for both
        /// EndTime and StartTime are Now() [current date and time]
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [Localizable(true)]
        public System.DateTime EndTime
        {
            get
            {
                return this.endTime_;
            }

            set
            {
                this.endTime_ = value;
            }
        }

        private String text_;
        /// <summary>
        /// OVERRIDE THE DEFAULT CALCULATIONS WITH A TEXT FIELD.
        /// This is intended for data messages that have no updates
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(true)]
        public String Text
        {
            get
            {
                return this.text_;
            }

            set
            {
                this.text_ = value;
            }
        }

        private bool useTextInsteadOfDates_;

        /// <summary>
        /// The value of this field tells the control whether
        /// or not to override the date/time caluclations with a
        /// text message.
        /// </summary>
        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool UseTextInsteadOfDates
        {
            get { return this.useTextInsteadOfDates_; }
            set { this.useTextInsteadOfDates_ = value; }
        }

        /// <summary>
        /// Generates the output string based upon the date/time inputs
        /// </summary>
        /// <returns>String to be printed by the RendersContents method</returns>
        protected String generateOutputText()
        {
            System.TimeSpan ts = EndTime.Subtract(StartTime);
            if (ts.Days == 0)
            {
                if (ts.Hours == 0)
                {
                    if (ts.Minutes == 0)
                    {
                        return formatString(ts.Seconds, Resources.Strings.UnitSeconds);
                    }
                    else
                    {
                        return formatString(ts.Minutes, Resources.Strings.UnitMinutes);
                    }
                }
                else
                {
                    return formatString(ts.Hours, Resources.Strings.UnitHours);
                }
            }
            else
            {
                return formatString(ts.Days, Resources.Strings.UnitDays);
            }
        }

        /// <summary>
        /// Formats the final string
        /// </summary>
        /// <param name="timeValue">displacement value</param>
        /// <param name="unitName">displacement unit</param>
        /// <returns>Formatted final string</returns>
        protected String formatString(int timeValue, String unitName)
        {
            return (timeValue + " " + unitName + " " + Resources.Strings.SummaryWord);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (UseTextInsteadOfDates) //Check to see if we have a character override
                output.Write(Text);
            else
                output.Write(generateOutputText());
        }

        /// <summary>
        /// Protected member called by the constructors to handle the
        /// data member initializations for this control
        /// </summary>
        protected void initialize()
        {
            this.startTime_ = this.endTime_ = System.DateTime.Now;
            this.text_ = String.Empty;
            this.useTextInsteadOfDates_ = false;
        }

        public relativeTimeDifference() : base() {
            initialize();
        }

        public relativeTimeDifference(HtmlTextWriterTag tag)
            : base(tag)
        {
            initialize();
        }

        public relativeTimeDifference(string tag) : base(tag){
            initialize();
        }
    }
}
