using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCHMC.Core.Web.Scrambler.Helpers;
using CCHMC.Core.Web.Scrambler.Models;

namespace CCHMC.Core.Web.Scrambler.Test.Helpers
{
    [TestClass]
    public class DateTimeHelperUnitTest
    {
        [TestMethod]
        public void ApplyMaskValidNull()
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            DateTime time = DateTimeHelper.ApplyMask(now, null);
            Assert.AreEqual(now, time);
        }

        [TestMethod]
        public void ApplyMaskValidDate()
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            DateTime time = DateTimeHelper.ApplyMask(now, DateTimeMask.Date);
            Assert.AreEqual(now.Date, time);
        }

        [TestMethod]
        public void ApplyMaskValidTime()
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            DateTime time = DateTimeHelper.ApplyMask(now, DateTimeMask.Time);
            Assert.AreEqual(new DateTime(now.TimeOfDay.Ticks), time);
        }

        [TestMethod]
        public void ApplyMaskValidDateTime()
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            DateTime time = DateTimeHelper.ApplyMask(now, DateTimeMask.DateTime);
            Assert.AreEqual(now, time);
        }

        [TestMethod]
        public void GenerateDateTimeValidMask()
        {
            DateTimeMask mask = DateTimeMask.Time;
            DateTime time = DateTimeHelper.GenerateDateTime(mask);
            Assert.IsTrue(time.Ticks <= TimeSpan.TicksPerDay);
        }

        [TestMethod]
        public void GenerateDateTimeEmptyMaskZeroStep()
        {
            DateTimeMask mask = new DateTimeMask();
            DateTime time = DateTimeHelper.GenerateDateTime(mask, TimeSpan.Zero);
            Assert.AreEqual(new DateTime(ConstantValues.MinDateTimeTicks), time);
        }

        [TestMethod]
        public void GenerateDateTimeValidMaskZeroStep()
        {
            DateTimeMask mask = DateTimeMask.Time;
            DateTime time = DateTimeHelper.GenerateDateTime(mask, TimeSpan.Zero);
            Assert.AreEqual(new DateTime(ConstantValues.MinDateTimeTicks % TimeSpan.TicksPerDay), time);
        }

        [TestMethod]
        public void GenerateDateTimeOversizedStep()
        {
            DateTimeMask mask = DateTimeMask.Time;
            TimeSpan step = new TimeSpan(TimeSpan.TicksPerDay * 2);
            DateTime time = DateTimeHelper.GenerateDateTime(mask, step);
            Assert.AreEqual(new DateTime(ConstantValues.MinDateTimeTicks % TimeSpan.TicksPerDay), time);
        }

        [TestMethod]
        public void GenerateDateTimeEmptyMaskValidStep()
        {
            DateTimeMask mask = new DateTimeMask();
            TimeSpan step = new TimeSpan(2, 30, 0);
            DateTime time = DateTimeHelper.GenerateDateTime(mask, step);
            Assert.AreEqual(0, time.Ticks % step.Ticks);
        }

        [TestMethod]
        public void GenerateDateTimeValidMaskValidStep()
        {
            DateTimeMask mask = DateTimeMask.Time;
            TimeSpan step = new TimeSpan(2, 30, 0);
            DateTime time = DateTimeHelper.GenerateDateTime(mask, step);
            Assert.AreEqual(0, time.Ticks % step.Ticks);
            Assert.IsTrue(time.Ticks <= TimeSpan.TicksPerDay);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValue00()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2000, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 100);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValue5()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2005, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 5);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValue10()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2010, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 10);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValue25()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2025, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 25);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValue50()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2050, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 25);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValue75()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2075, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 25);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValueAlt5()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2085, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 5);
        }

        [TestMethod]
        public void StrictObfuscationCriticalYearValueAlt10()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(2090, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Year % 10);

        }

        [TestMethod]
        public void StrictObfuscationCriticalMonthValue3()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, 3, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Month % 3);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMonthValue6()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, 6, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Month % 6);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMonthValue9()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, 9, time.Day, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Month % 3);

        }

        [TestMethod]
        public void StrictObfuscationCriticalDayValue5()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, 5, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Day % 5);
        }

        [TestMethod]
        public void StrictObfuscationCriticalDayValue10()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, 10, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Day % 10);
        }

        [TestMethod]
        public void StrictObfuscationCriticalDayValue15()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, 15, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Day % 15);
        }

        [TestMethod]
        public void StrictObfuscationCriticalDayValue30()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, 30, time.Hour, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Day % 15);
        }

        [TestMethod]
        public void StrictObfuscationCriticalHourValue3()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, 3, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Hour % 3);
        }

        [TestMethod]
        public void StrictObfuscationCriticalHourValue6()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, 6, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Hour % 6);
        }

        [TestMethod]
        public void StrictObfuscationCriticalHourValue9()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, 9, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Hour % 3);
        }

        [TestMethod]
        public void StrictObfuscationCriticalHourValue12()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, 12, time.Minute, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Hour % 6);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMinuteValue5()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 5, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Minute % 5);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMinuteValue10()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 10, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Minute % 10);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMinuteValue15()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 15, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Minute % 15);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMinuteValue30()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 30, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Minute % 30);
        }

        [TestMethod]
        public void StrictObfuscationCriticalMinuteValue45()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 45, time.Second);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Minute % 15);
        }

        [TestMethod]
        public void StrictObfuscationCriticalSecondValue5()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 5);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Second % 5);

        }

        [TestMethod]
        public void StrictObfuscationCriticalSecondValue10()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 10);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Second % 10);
        }

        [TestMethod]
        public void StrictObfuscationCriticalSecondValue15()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 15);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Second % 15);
        }

        [TestMethod]
        public void StrictObfuscationCriticalSecondValue30()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 30);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Second % 30);
        }

        [TestMethod]
        public void StrictObfuscationCriticalSecondValue45()
        {
            DateTime time = DateTime.MinValue;
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 45);
            var obf = DateTimeHelper.StrictObfuscation(time, DateTimeMask.DateTime);
            Assert.AreEqual(0, ((DateTime)obf).Second % 15);
        }

        [TestMethod]
        public void StrictObfuscationStringEmptyMask()
        {
            DateTimeMask mask = new DateTimeMask();
            DateTime time = DateTimeHelper.StrictObfuscation(new DateTime(5, 5, 5, 5, 5, 5).ToString(), mask);
            Assert.AreNotEqual(time, new DateTime(5, 5, 5, 5, 5, 5));
        }

        [TestMethod]
        public void StrictObfuscationStringValidMask()
        {
            DateTimeMask mask = DateTimeMask.Date;
            DateTime time = DateTimeHelper.StrictObfuscation(new DateTime(5, 5, 5, 5, 5, 5).ToString(), mask);
            Assert.AreNotEqual(time, new DateTime(5, 5, 5, 5, 5, 5).Date);
            Assert.AreEqual(time.TimeOfDay, DateTime.MinValue.TimeOfDay);
        }

        [TestMethod]
        public void StrictObfuscationDateEmptyMask()
        {
            DateTimeMask mask = new DateTimeMask();
            DateTime time = DateTimeHelper.StrictObfuscation(new DateTime(5, 5, 5, 5, 5, 5), mask);
            Assert.AreNotEqual(time, new DateTime(5, 5, 5, 5, 5, 5));
        }

        [TestMethod]
        public void StrictObfuscationDateValidMask()
        {
            DateTimeMask mask = DateTimeMask.Date;
            DateTime time = DateTimeHelper.StrictObfuscation(new DateTime(5, 5, 5, 5, 5, 5), mask);
            Assert.AreNotEqual(time, new DateTime(5, 5, 5, 5, 5, 5).Date);
            Assert.AreEqual(time.TimeOfDay, DateTime.MinValue.TimeOfDay);
        }

        [TestMethod]
        public void StrictObfuscationInvalidEmptyMask()
        {
            DateTimeMask mask = new DateTimeMask();
            DateTime time = DateTimeHelper.StrictObfuscation(new object(), mask);
            Assert.AreNotEqual(DateTime.MinValue, time);
        }

        [TestMethod]
        public void StrictObfuscationInvalidValidMask()
        {
            DateTimeMask mask = DateTimeMask.Date;
            DateTime time = DateTimeHelper.StrictObfuscation(new object(), mask);
            Assert.AreNotEqual(DateTime.MinValue, time);
        }
    }
}
