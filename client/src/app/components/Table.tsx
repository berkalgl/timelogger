import moment from 'moment';
import React from 'react';

const useSortableData = (items, config) => {
	const [sortConfig, setSortConfig] = React.useState(config);

	const sortedItems = React.useMemo(() => {
		if (!items)
			return []
		let sortableItems = [...items];
		if (sortConfig !== null) {
			sortableItems.sort((a, b) => {
				if (a[sortConfig.key] < b[sortConfig.key]) {
					return sortConfig.direction === 'ascending' ? -1 : 1;
				}
				if (a[sortConfig.key] > b[sortConfig.key]) {
					return sortConfig.direction === 'ascending' ? 1 : -1;
				}
				return 0;
			});
		}
		return sortableItems;
	}, [items, sortConfig]);

	const requestSort = (key) => {
		let direction = 'ascending';
		if (
			sortConfig &&
			sortConfig.key === key &&
			sortConfig.direction === 'ascending'
		) {
			direction = 'descending';
		}
		setSortConfig({ key, direction });
	};

	return { items: sortedItems, requestSort, sortConfig };
};

export default function Table(props) {
	const {
		columns,
		data,
		innerDataName,
		innerComponent,
		parentIdName,
		parentId,
		isAdd,
		addForm
	} = props;
	const [showAddModal, setShowAddModal] = React.useState(false)
	const { items, requestSort, sortConfig } = useSortableData(data, null);
	const getClassNamesFor = (name) => {
		if (!sortConfig) {
			return;
		}
		return sortConfig.key === name ? sortConfig.direction : undefined;
	};

	if (!columns) {
		return (<></>)
	}
	return (
		<table className="table-fixed w-full">
			<thead className="bg-gray-200">
				<tr key='tablehead'>
					{
						columns.map((column, i) => {
							return (
								<th key={"columnHead" + i} className={column.classes}>
									{column.sort ?
										<button
											type="button"
											onClick={() => requestSort(column.dataField)}
											className={getClassNamesFor(column.dataField)}
										>
											{column.text}
										</button>
										:
										<span>{column.text}</span>
									}
								</th>
							)
						})
					}
					{
						isAdd &&
						<th key={"addColumnButton" + parentId} className='border px-4 py-2'>
							<button
								key="addColumnButton"
								id={"id" + parentId}
								type="button"
								onClick={() => setShowAddModal(true)}
							>
								Add
							</button>
							{addForm && React.cloneElement(addForm, { parentId: parentId, showModal: showAddModal, onClose: () => setShowAddModal(false), refresh: () => addForm.props.refresh() })}
						</th>
					}
				</tr>
			</thead>
			<tbody>
				{items.map((row, i) => {
					return (
						<>
							<tr key={parentId + i} data-toggle="collapse" data-target={'#' + innerDataName + i} className="accordion-toogle">
								{
									columns.map((column, j) => {
										return (
											<td key={i + j} className={column.classes}>
												{column.isDate ? moment(row[column.dataField]).utc().format(column.dateFormat) : row[column.dataField]}
											</td>
										)
									})
								}
								{
									isAdd &&
									<td key={i} className='border px-4 py-2'>
										Detail
									</td>
								}
							</tr>
							{innerDataName &&
								<tr key={innerDataName + i}>
									<td colSpan={4} style={{ padding: '0 !important' }}>
										<div className="accordian-body collapse" id={innerDataName + i} style={{ marginLeft: '20px', marginRight: '20px' }}>
											{innerComponent && React.cloneElement(innerComponent, { parentId: row[parentIdName] })}
										</div>
									</td>
								</tr>
							}

						</>
					)
				})}
			</tbody>
		</table>
	);
}