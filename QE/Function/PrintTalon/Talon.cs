using Microsoft.EntityFrameworkCore;
using QE.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QE.Function.PrintTalon;

public class Talon
{
    /// <summary>
    /// печать талона
    /// </summary> 
    public static async void PrintTalon(object sender, RoutedEventArgs e, SService sService, string Ip)
    {
        try
        {
            EqContext eqContext = new EqContext();

            FastReport.Report report = new FastReport.Report();
            var path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory))) + "\\FastReport\\Operator.frx";
            report.Load(path);
            var LastTicketNumber =  eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == Ip && s.SServiceId == sService.Id && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).OrderByDescending(o => o.TicketNumber).Select(s => s.TicketNumber).FirstOrDefault();

            DTicket dTicket_New = new DTicket();
            dTicket_New.SOfficeId = eqContext.SOfficeTerminals.First(s => s.IpAddress == Ip).SOfficeId;
            dTicket_New.SOfficeTerminalId = eqContext.SOfficeTerminals.First(s => s.IpAddress == Ip).Id;
            dTicket_New.SServiceId = sService.Id;
            dTicket_New.ServicePrefix = sService.ServicePrefix;
            //dTicket.SPriorityId = 1; 
            dTicket_New.TicketNumber = LastTicketNumber + 1;
            dTicket_New.TicketNumberFull = sService.ServicePrefix + (LastTicketNumber + 1);
            //dTicket.DTicketPrerecordId = 1;
            dTicket_New.SStatusId = 1;
            //dTicket.SEmployeeId = 1;
            //dTicket.SOfficeWindowId = 1;
            dTicket_New.DateRegistration = DateOnly.FromDateTime(DateTime.Now);
            dTicket_New.TimeRegistration = TimeOnly.FromDateTime(DateTime.Now);

            DTicketStatus dTicketStatus = new DTicketStatus
            {
                //DTicketId = eqContext.DTickets.First(s => s.SOfficeTerminal.IpAddress == Ip && s.DateRegistration == dTicket_New.DateRegistration && s.TimeRegistration == dTicket_New.TimeRegistration).Id,
                SStatusId = 1
            };

            dTicket_New.DTicketStatuses.Add(dTicketStatus);

            eqContext.DTickets.Add(dTicket_New);
            eqContext.SaveChanges();

            report.SetParameterValue("Operation", sService.ServiceName);
            report.SetParameterValue("Number", dTicket_New.TicketNumberFull);
            report.SetParameterValue("Time", dTicket_New.TimeRegistration);
            report.SetParameterValue("TotalQueue", eqContext.DTickets.Where(s => s.SOfficeTerminal.IpAddress == Ip && s.DateRegistration == DateOnly.FromDateTime(DateTime.Now)).Count());
            report.SetParameterValue("BeforeCount", LastTicketNumber);
            report.SetParameterValue("MFC", eqContext.SOffices.First(l => l.Id == eqContext.SOfficeTerminals.First(g => g.IpAddress == Ip).SOfficeId).OfficeName);
            report.Prepare();
            report.PrintSettings.ShowDialog = false;
            report.PrintSettings.PrintOnSheetRawPaperSize = 0;
            await Client.SendMessageAsync("new Ticket", Ip);

            try
            {
                report.Print();
            }
            catch (Exception ex) {
            
            } 
        }
        catch (Exception ex)
        {

        }
    }
}