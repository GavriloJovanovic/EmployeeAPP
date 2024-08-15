export class Employee {
    Id: string;
    EmployeeName: string;
    StarTimeUtc: Date;
    EndTimeUtc: Date;
    totalTime: Number = 0;
    EntryNotes: string;
  
    constructor(id: string, employeeName: string, startTimeUtc: Date, endTimeUtc: Date, entryNotes: string) {
      this.Id = id;
      this.EmployeeName = employeeName;
      this.StarTimeUtc = startTimeUtc;
      this.EndTimeUtc = endTimeUtc;
      this.EntryNotes = entryNotes;
      this.totalTime = this.getTotalTimeInHours()
    }
  
    public getTotalTimeInHours(): number {
      return (this.EndTimeUtc.getTime() - this.StarTimeUtc.getTime()) / (1000 * 60 * 60);
    }
  }