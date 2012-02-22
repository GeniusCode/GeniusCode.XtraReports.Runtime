using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using FluentAssertions;
using GeniusCode.XtraReports.Runtime.Actions;
using NUnit.Framework;

namespace GeniusCode.XtraReports.Runtime.Tests.Unit
{
    [TestFixture]
   public class ReportControlActionFacade_tests
    {
        [Test]
        public void Should_cast_correctly_when_single_action_of_one_type()
        {
            var label = new XRLabel() {Text = "uncoverted"};
            var action = ReportControlAction<XRLabel>.WithNoPredicate(l => l.Text = "Converted");
            var facade = new ReportControlActionFacade(action);
            facade.AttemptActionsOnControl(label);

            label.Text.Should().Be("Converted");
        }

        [Test]
        public void Should_cast_correctly_when_multiple_actions_of_multiple_types()
        {
            var label = new XRLabel() { Text = "uncoverted" };
            var action = ReportControlAction<XRLabel>.WithNoPredicate(l => l.Text = "Converted");
            var action2 = ReportControlAction<XRLine>.WithNoPredicate(l => l.ForeColor = Color.Gold);
            var facade = new ReportControlActionFacade(action,action2);
            facade.AttemptActionsOnControl(label);

            label.Text.Should().Be("Converted");
        }


        [Test]
        public void Should_only_apply_action_when_predicate_is_satisfied()
        {
            const string transformText = "Jeremiah";
            var action = new ReportControlAction<XRLabel>((l) => l.Text != string.Empty, (l) => l.Text = transformText);

            var facade = new ReportControlActionFacade(action);

            var label1 = new XRLabel { Text = string.Empty };
            var label2 = new XRLabel { Text = "ChangeMe" };

            facade.AttemptActionsOnControl(label1);
            facade.AttemptActionsOnControl(label2);

            Assert.AreNotEqual(transformText, label1.Text);
            Assert.AreEqual(transformText, label2.Text);
        }

    }
}
