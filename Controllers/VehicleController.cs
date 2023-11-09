using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VehicleManagement.Data;
using VehicleManagement.Models;

namespace VehicleManagement.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IConfiguration _configuration;

        public VehicleController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Vehicle
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("VehicleViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }

            return View(dtbl);
        }

        // GET: Vehicle/AddOrEdit
        public IActionResult AddOrEdit(int? id)
        {
            VehicleViewModel vehicleViewModel = new VehicleViewModel();
            if (id > 0)
                vehicleViewModel = FetchBookById(id);
            return View(vehicleViewModel);
        }

        // POST: Vehicle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("Id,VehicleModel,Seat,Color")] VehicleViewModel vehicleViewModel)
        {

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("VehicleAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("Id", vehicleViewModel.Id);
                    sqlCmd.Parameters.AddWithValue("VehicleModel", vehicleViewModel.VehicleModel);
                    sqlCmd.Parameters.AddWithValue("Seat", vehicleViewModel.Seat);
                    sqlCmd.Parameters.AddWithValue("Color", vehicleViewModel.Color);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleViewModel);
        }

        // GET: Vehicle/Delete/5
        public IActionResult Delete(int? id)
        {
            VehicleViewModel vehicleViewModel = FetchBookById(id);
            return View(vehicleViewModel);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("VehicleDeleteById", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("Id",id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        public VehicleViewModel FetchBookById(int? id)
        {
            VehicleViewModel vehicleViewModel = new VehicleViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("VehicleViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("Id", id);
                sqlDa.Fill(dtbl);
                if(dtbl.Rows.Count == 1)
                {
                    vehicleViewModel.Id = Convert.ToInt32(dtbl.Rows[0]["ID"].ToString());
                    vehicleViewModel.VehicleModel = dtbl.Rows[0]["VehicleModel"].ToString();
                    vehicleViewModel.Seat = Convert.ToInt32(dtbl.Rows[0]["Seat"].ToString());
                    vehicleViewModel.Color = dtbl.Rows[0]["Color"].ToString();
                }
                return vehicleViewModel;
            }

        }
    }
}
