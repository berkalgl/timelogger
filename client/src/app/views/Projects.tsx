import React from 'react';
import { Form } from 'react-bootstrap';
import Modal from '../components/Modal';
import Table from '../components/Table';
import { Get, Post, Put } from '../api/ApiHelper';
import Timesheets from './Timesheets';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css"

const projectGetApi = "/Projects?name=";
const projectAddApi = "/Projects/AddProject";
const projectDisableApi = "/Projects/DisableProject/";

const fixTimezoneOffset = (date?: Date): string => {
    if (!date) return "";
    return new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toJSON();
}

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
	dateFormat: 'DD/MM/yyyy',
	sort: true
}, {
	dataField: 'totalHoursWorked',
	text: 'Total Hours',
	classes: 'border px-4 py-2',
}, {
	dataField: 'isDeleted',
	text: 'Disabled',
	classes: 'border px-4 py-2',
	innerCell: (row, refresh) => ( <DisableButton row={row} refresh={refresh}></DisableButton>
	)
}];

const DisableButton = (props) => {
	const {row, refresh} = props
	const [disable, setDisable] = React.useState(row["isDeleted"])
	const [buttonText, setButtonText] = React.useState(row["isDeleted"] ? "Disabled":"Disable It")

	const handleProjectDisable = async (id, refresh) => {
		await Put(projectDisableApi + id,
			(data) => {
				alert(data.message)
				if (data.state === 0)
				{
					refresh();
					setDisable(true)
					setButtonText("Disabled")
				}
			},
			(err) => {
				alert('An error occured while fetching' + err)
			})
	};

	if(disable)
	{
		return(
			<div>
				{buttonText}
			</div>
		)
	}

	return (
		<div>
			<button 
				className = {"bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"}
				onClick={e => handleProjectDisable(row["id"], refresh)}>{buttonText}</button>
		</div>
	)
}


const ProjectAddForm = props => {
    const { showModal, onClose, refresh } = props

	const [nameValue, setNameValue] = React.useState("")
    const [deadlineValue, setDeadlineValue] = React.useState<Date | undefined>(undefined)

	const handleProjectSubmit = async (e) => {

		e.preventDefault();
		const project = {
			name: nameValue,
			deadline: fixTimezoneOffset(deadlineValue)
		}
		await Post(projectAddApi, project,
			(data) => {
				alert(data.message)
				refresh()
			},
			(err) => {
				alert('An error occured while fetching' + err)
			})
	};


	return (
		<Modal show={showModal} handleClose={onClose} title="Add Project" handleSave={handleProjectSubmit}>
			<Form>
				<Form.Group className="mb-3" controlId="projectForm.Name">
					<Form.Label>Name</Form.Label>
					<Form.Control
						type="text"
						onChange={(e) => setNameValue(e.target.value)}
					/>
				</Form.Group>
				<Form.Group controlId="projectForm.Deadline">
					<Form.Label>Deadline</Form.Label>
                    <DatePicker
                        selected={deadlineValue}
                        onChange={(date) => setDeadlineValue(date ?? undefined)}
                        dateFormat="dd/MM/yyyy"
                    />
				</Form.Group>
			</Form>
		</Modal>
	)
}
export default function Projects() {

	// init
	const [sourceData, setData] = React.useState([]);
	const [searchText, setSearchText] = React.useState("");
	const [sourceUrl, setSourceUrl] = React.useState(projectGetApi);
	const [showModal, setShowModal] = React.useState(false);
	const [refreshKey, setRefreshKey] = React.useState(0);

	//functions for functional component
	function HandleSearchClick(e) {
		e.preventDefault()
		setSourceUrl(projectGetApi + searchText)
	}

	const handleClose = () => setShowModal(false);
	const handleShow = () => setShowModal(true);
	const handleRefresh = () => setRefreshKey(refreshKey + 1);
	React.useEffect(() => {
		const fetchData = async () => {
			await Get(sourceUrl,
				(data) => {
					setData(data);
				},
				(err) => {
					alert('An error occured while fetching' + err)
				})

		}
		fetchData();
	}, [sourceUrl, refreshKey]);

	return (
		<>
			<div className="flex items-center my-6">
				<div className="w-1/2">
					<button id="projectAddBtn" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={handleShow}>Add entry</button>
				</div>


				<div className="w-1/2 flex justify-end">
					<form>
						<input className="border rounded-full py-2 px-4" type="search" placeholder="Search" aria-label="Search" onChange={event => setSearchText(event.target.value)} />
						<button className="bg-blue-500 hover:bg-blue-700 text-white rounded-full py-2 px-4 ml-2" type="submit" onClick={HandleSearchClick}>Search</button>
					</form>
				</div>
			</div>

			<Table
				data={sourceData}
				columns={columns}
				innerDataName="timesheets"
				innerComponent={<Timesheets/>}
				refresh = {handleRefresh}
			/>

			<ProjectAddForm showModal={showModal} onClose={handleClose} refresh={handleRefresh}></ProjectAddForm>
		</>
	);
}

