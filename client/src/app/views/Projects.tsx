import React from 'react';
import Table from '../components/Table';

const timesheetColumns = [{
	dataField: 'id',
	text: '#',
	classes: 'border px-4 py-2 w-12',
	keyField: true
  }, {
	dataField: 'startTime',
	text: 'Start Time',
	classes: 'border px-4 py-2',
	isDate: true,
	dateFormat : 'DD/MM/yyyy hh:mm',
	sort: true
  }, {
	dataField: 'endTime',
	text: 'End Time',
	classes: 'border px-4 py-2',
	isDate: true,
	dateFormat : 'DD/MM/yyyy hh:mm',
	sort: true
  }, {
	dataField: 'comment',
	text: 'Comment',
	classes: 'border px-4 py-2',
  }];


const columns = [{
	dataField: 'id',
	text: '#',
	classes: 'border px-4 py-2 w-12',
	keyField: true
  }, {
	dataField: 'name',
	text: 'Name',
	classes: 'border px-4 py-2'
  }, {
	dataField: 'deadline',
	text: 'Deadline',
	classes: 'border px-4 py-2',
	isDate: true,
	dateFormat : 'DD/MM/yyyy',
	sort: true
  }, {
	dataField: 'totalHoursWorked',
	text: 'Total Hours',
	classes: 'border px-4 py-2',
  }];

export default function Projects() {
	const [sourceData, setData] = React.useState([]);
	const [sourceUrl, setSourceUrl] = React.useState("http://localhost:3001/api/Projects");
	
	React.useEffect(() => {
		const fetchData = async () => {
			await fetch(sourceUrl)
			.then(results => results.json())
			.then(data => {
			  setData(data.result);
			});
		}
		fetchData();
	
	  }, [sourceUrl]);

	if(!sourceData)
	{
		return (<h3>Loading...</h3>)
	}
	return (
		<>
			<div className="flex items-center my-6">
				<div className="w-1/2">
					<button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Add entry</button>
				</div>

				<div className="w-1/2 flex justify-end">
					<form>
						<input className="border rounded-full py-2 px-4" type="search" placeholder="Search" aria-label="Search" />
						<button className="bg-blue-500 hover:bg-blue-700 text-white rounded-full py-2 px-4 ml-2" type="submit">Search</button>
					</form>
				</div>
			</div>

			<Table data={sourceData} columns={columns} timesheetColumns = {timesheetColumns} />
		</>
	);
}