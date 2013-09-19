using System.Web;
using System.Web.Optimization;

namespace Validus.Console
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
		{
			//BundleTable.EnableOptimizations = true;

			// TODO: bundles.Add(new ScriptBundle("~/bundles/home").Include
			// TODO: bundles.Add(new ScriptBundle("~/bundles/admin").Include

			bundles.Add(new ScriptBundle("~/bundles/min").Include(
						"~/scripts/lib/jquery-{version}.js",
						"~/scripts/lib/jquery-ui-{version}.js",
						"~/scripts/lib/moment.js",
						"~/scripts/lib/jquery.validate.js",
						"~/scripts/lib/jquery.validate.unobtrusive.js",
						"~/scripts/lib/knockout-{version}.js",
						"~/scripts/lib/bootstrap.js",
						"~/scripts/lib/bootstrap-typeahead.js",
                        "~/scripts/lib/kendo.dataviz.js",
						"~/scripts/lib/toastr.custom.js"));

			bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/scripts/lib/jquery.pubsub.js",
						"~/scripts/lib/jquery.datatables.js",
						"~/scripts/lib/jquery.bootstrap.datatables.js",
						"~/scripts/lib/jquery.ui.combobox.js"));

			bundles.Add(new ScriptBundle("~/bundles/app").Include(
						"~/scripts/app/helpers.js",
                        "~/scripts/app/ko.InitialiseDatePickers.js",
                        "~/scripts/app/ko.InitialisePane.js",
                        "~/scripts/app/ko.InitialiseTabs.js",
                        "~/scripts/app/ko.InitialiseTypeaheads.js",
						"~/scripts/app/ko.bootstrap-carousel.js",
						"~/scripts/app/ko.bootstrap-tabs.js",
                        "~/Scripts/app/ko.dirtyFlag.js",
                        "~/Scripts/app/ko.dumpObservable.js",
                        "~/Scripts/app/ko.cancelBubbleClickEvent.js",
                        "~/Scripts/app/ko.editPageGrid.js",
                        "~/Scripts/app/ko.pageGrid.js",
                        "~/Scripts/app/knockout-datatables.js",
						"~/scripts/app/ko.jquery-datatables.js"));

			bundles.Add(new ScriptBundle("~/bundles/vm").Include(
                        "~/scripts/app/vmsubmissionbase.js",
                        "~/scripts/app/vmsubmissionen.js",
                        "~/scripts/app/vmsubmissionpv.js",
                        "~/scripts/app/vmsubmissionfi.js",
                        "~/scripts/app/vmsubmissionexampleenergy.js",
                        "~/scripts/app/dtos/teammembershipteamdto.js",
                        "~/scripts/app/dtos/teammembershipdto.js",
                        "~/scripts/app/dtos/basicuserdto.js",
                        "~/scripts/app/dtos/userdto.js",
                        "~/scripts/app/dtos/linkdto.js",
                        "~/scripts/app/dtos/teamdto.js",
                        "~/scripts/app/dtos/cobdto.js",
                        "~/scripts/app/dtos/defaultunderwriterdto.js",
                        "~/scripts/app/dtos/officedto.js",
                        "~/scripts/app/dtos/teamlinksdto.js",
                        "~/scripts/app/dtos/TeamQuoteTemplatesDto.js",
                        "~/scripts/app/dtos/TeamAppAcceleratorsDto.js",
                        "~/scripts/app/dtos/MarketWordingSettingDto.js",
                        "~/scripts/app/dtos/TeamMarketWordingsDto.js",
                        "~/scripts/app/dtos/TeamTermsNConditionWordingsDto.js",
                        "~/scripts/app/dtos/TermsNConditionWordingSettingDto.js",
                        "~/scripts/app/dtos/SubjectToClauseWordingSettingDto.js",
                        "~/scripts/app/dtos/TeamSubjectToClauseWordingsDto.js",
                        "~/scripts/app/dtos/submissiondto.js",
                        "~/scripts/app/models/adminview.js",
                        "~/scripts/app/models/Accelerator.js",
                        "~/scripts/app/models/AuditTrail.js",
                        "~/scripts/app/models/QuoteTemplate.js",
                        "~/scripts/app/models/MarketWording.js",
                        "~/scripts/app/models/TermsNConditionWording.js",
                        "~/scripts/app/models/SubjectToClauseWording.js",
                        "~/scripts/app/vmadminindex.js",
                        "~/scripts/app/vmmanageteams.js",
                        "~/scripts/app/vmmanageusers.js",
                        "~/scripts/app/vmmanagelinks.js",
                        "~/scripts/app/vmManageAccelerators.js",
                        "~/scripts/app/vmManageQuoteTemplates.js",
                        "~/scripts/app/vmManageMarketWordings.js",
                        "~/scripts/app/vmManageTermsNConditionWordings.js",
                        "~/scripts/app/vmManageSubjectToClauseWordings.js"));

			bundles.Add(new ScriptBundle("~/bundles/main").Include(
                        "~/scripts/main.js", 
                        "~/scripts/main_DebugActionLinks.js",
                        "~/scripts/mainHelpers.js"));

			bundles.Add(new StyleBundle("~/bundles/css").Include(
						"~/content/css/bootstrap.themed.css",
						"~/content/css/bootstrap-responsive.css",
						"~/content/css/jquery.bootstrap.datatables.css",
						"~/content/css/toastr.custom.css",
						"~/content/css/jquery-ui-1.10.0.custom.css",
						"~/content/css/jquery.ui.1.10.0.ie.css",
						"~/content/css/bootstrap.custom.css",
                        "~/content/css/kendo.dataviz.blueopal.css",
						"~/content/css/main.css"));
		}
    }
}