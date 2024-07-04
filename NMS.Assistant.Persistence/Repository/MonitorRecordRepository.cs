using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NMS.Assistant.Persistence.Repository
{
    public class MonitorRecordRepository: IMonitorRecordRepository
    {
        private readonly NmsAssistantContext _db;

        public MonitorRecordRepository(NmsAssistantContext db)
        {
            _db = db;
        }
        
        public Task<ResultWithValue<List<MonitorRecord>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ResultWithValue<List<MonitorStatusRecord>>> Search(DateTime startDate, DateTime endDate)
        {
            List<MonitorStatusRecord> tracked = new List<MonitorStatusRecord>();

            Result storedProcResult = await StoredProcedureRepository.Execute(_db,
                    StoredProcedure.GetMonitorStatus,
                    new List<StoredProcedureParameter>
                    {
                        StoredProcedureParameter.NullableDateParam("@startDate", startDate),
                        StoredProcedureParameter.NullableDateParam("@endDate", endDate),
                    },
                    dbDataSetReader: dataSet =>
                    {
                        for (int tableIndex = 0; tableIndex < (dataSet.Tables.Count / 2); tableIndex++)
                        {
                            int actualIndex = tableIndex * 2;
                            DataTable headingTable = dataSet.Tables[actualIndex];
                            MonitorStatusRecord mainRecord = MonitorStatusRecord.FromDataRow(headingTable.Rows[0]);

                            DataTable resultsTable = dataSet.Tables[actualIndex + 1];
                            foreach (DataRow row in resultsTable.Rows)
                            {
                                mainRecord.Ticks.Add(MonitorStatusRecordTicks.FromDataRow(row));
                            }

                            tracked.Add(mainRecord);
                        }
                    }
                );

            return new ResultWithValue<List<MonitorStatusRecord>>(tracked.Count > 0, tracked, string.Empty);
        }

        public async Task<Result> Add(MonitorRecord addItem)
        {
            try
            {
                await _db.MonitorRecords.AddAsync(addItem);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public Task<Result> Edit(MonitorRecord editItem)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
