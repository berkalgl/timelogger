import React from 'react';
import { Form } from 'react-bootstrap';
import { Get, Post } from '../api/ApiHelper';
import Modal from '../components/Modal';
import Table from '../components/Table';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css"

const timesheetGetApi = "/Timesheets?projectId=";
const timesheetAddApi = "/Timesheets/AddTimesheet";

const columns = [{
    dataField: 'id',
    text: '#',
    classes: 'border px-4 py-2 w-12',
    keyField: true
}, {
    dataField: 'startTime',
    text: 'Start Time',
    classes: 'border px-4 py-2',
    isDate: true,
    dateFormat: 'DD/MM/yyyy HH:mm',
    sort: true
}, {
    dataField: 'endTime',
    text: 'End Time',
    classes: 'border px-4 py-2',
    isDate: true,
    dateFormat: 'DD/MM/yyyy HH:mm',
    sort: true
}, {
    dataField: 'comment',
    text: 'Comment',
    classes: 'border px-4 py-2',
}];

const fixTimezoneOffset = (date?: Date): string => {
    if (!date) return "";
    return new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toJSON();
}

const TimesheetModal = props => {
    const { parentId, showModal, onClose, refresh } = props

    const [commentValue, setCommentValue] = React.useState("")
    const [startTime, setStartTime] = React.useState<Date | undefined>(undefined)
    const [endTime, setEndTime] = React.useState<Date | undefined>(undefined)

    const handleTimesheetSubmit = async (e) => {
        e.preventDefault();
        const timesheet = {
            comment: commentValue,
            startTime: fixTimezoneOffset(startTime),
            endTime: fixTimezoneOffset(endTime),
            projectId: parentId
        }
        await Post(timesheetAddApi, timesheet,
            (data) => {
                alert(data.message)
                if(data.state === 0)
                {
                    refresh()
                }
            },
            (err) => {
                alert('An error occured while fetching' + err)
            })
    };

    return (
        <Modal key={"addTimesheetModal" + parentId} show={showModal} handleClose={onClose} title="Add Timesheet" handleSave={handleTimesheetSubmit}>
            <Form>
                <Form.Group className="mb-3" controlId="timesheetForm.Comment">
                    <Form.Label>Comment</Form.Label>
                    <Form.Control
                        value={commentValue}
                        type="text"
                        onChange={(e) => setCommentValue(e.target.value)}
                    />
                </Form.Group>
                <Form.Group controlId="timesheetForm.StartTime">
                    <Form.Label>Start Time</Form.Label>
                    <DatePicker
                        selected={startTime}
                        onChange={(date) => setStartTime(date ?? undefined)}
                        showTimeSelect
                        timeFormat="HH:mm"
                        timeIntervals={30}
                        timeCaption="time"
                        dateFormat="dd/MM/yyyy HH:mm"
                    />
                </Form.Group>
                <Form.Group controlId="timesheetForm.EndTime">
                    <Form.Label>End Time</Form.Label>
                    <DatePicker
                        selected={endTime}
                        onChange={(date) => setEndTime(date ?? undefined)}
                        showTimeSelect
                        timeFormat="HH:mm"
                        timeIntervals={30}
                        timeCaption="time"
                        dateFormat="dd/MM/yyyy HH:mm"
                    />
                </Form.Group>
            </Form>
        </Modal>
    );
}
export default function Timesheets(props) {
    // init
    const { parent } = props
    const parentId  = parent["id"]
    const parentDisabled = parent["isDeleted"]
    const [sourceData, setData] = React.useState([]);
    const [sourceUrl] = React.useState(timesheetGetApi);
    const [refreshKey, setRefreshKey] = React.useState(0);

    React.useEffect(() => {
        const fetchData = async () => {
            await Get(sourceUrl + parentId,
                (data) => {
                    setData(data);
                },
                (err) => {
                    alert('An error occured while fetching' + err)
                })

        }
        fetchData();
    }, [sourceUrl, parentId, refreshKey]);

    return (
        <>
            <Table
                columns={columns}
                data={sourceData}
                isAdd={!parentDisabled}
                addForm={<TimesheetModal refresh={() => {setRefreshKey(refreshKey + 1)}}/>}
                parentId={parentId}
            />
        </>
    );
}
