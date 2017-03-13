using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        /// <summary>
        /// Short name - used in the url part to indicate an active app (in host mode)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Full app name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A bit more verbal info on the application
        /// </summary>E:\MapHive\MapHive.Server\Core\DataModel\Application\_DataModel.cs
        public string Description { get; set; }

        /// <summary>
        /// The application's entry point(s).
        /// Usually there will only be one url, but during the dev or even in production, when there are multiple envs maintained, providing a pipe (|) separated list of urls,
        /// simplifies the app setup. App urls listed here are also used when establishing whether an app requires authentication.
        /// </summary>
        public string Urls { get; set; }

        /// <summary>
        /// Whether or not own application's splashscreen should be used, or the host should use own load mask
        /// </summary>
        public bool UseSplashscreen { get; set; }

        /// <summary>
        /// Whether or not the application requires authentication in order to be used.
        /// </summary>
        public bool RequiresAuth { get; set; }

        /// <summary>
        /// Whether or not the information about an application can be publicly accessible
        /// a common app means every user can get the information about the application such as its name, short name, description, url, etc. without having to authenticate
        /// if an application is marked as common it may be shown in the app switcher, so user can launch it (provided the env is configured for this of course)
        /// </summary>
        public bool IsCommon { get; set; }

        /// <summary>
        /// Whether or not an app is a default app;
        /// a default app is only meaningful in the context of an organisation.
        /// This means a default app should be an app that provides some dashboard like functionality
        /// for a user.
        /// Only one appliation can be flagged as Default
        /// </summary>
        public bool IsDefault { get; set; }


        /// <summary>
        /// Whether or not an app is a home app; Home app means this is an app loaded when there is no organisation context. A Home app would be like a main page - it should describe a project,
        /// perhaps give some videos, links and such.
        /// Only one application can be flagged as Home
        /// </summary>
        public bool IsHome { get; set; }


        /// <summary>
        /// Whether or not the application is a HOST application
        /// </summary>
        public bool IsHive { get; set; }
        
        /// <summary>
        /// Identifier of the app provider. It is assumed that a provider will have its account and be able to register and publish the apps via its dashboard.
        /// </summary>
        public Guid? ProviderId { get; set; }


        //TODO
        //comments
        //tags

        //ICON perhaps? base64png?


        //Payments API info
        //prices
        //payment periods - free, per use, day, x days, week, month, year
        //starts / expires dates, many price options
        //trial periods - same as payent period
        
        //payments should be logged independently per organisation(user), so it is possible to review what a user have paid for.

        //there should be an option to grant access to app / apps based on some sort of a subscription. maybe there should be some sort of payments API

        //when an app starts it should be possible to consult a payments api to check if a user with given org context can use an application!


    }
}
