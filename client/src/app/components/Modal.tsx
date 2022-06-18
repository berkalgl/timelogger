import React from 'react';

export default function Modal(props) {
	const {modalBody, title} = props;  
	return (
	<div className="modal" role="dialog">
		<div className="modal-dialog" role="document">
			<div className="modal-content">
				<div className="modal-header">
					<h5 className="modal-title">{title}</h5>
					<button type="button" className="close" data-dismiss="modal" aria-label="Close">
					<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<div className="modal-body">
					{modalBody}
				</div>
				<div className="modal-footer">
					<button type="button" className="btn btn-primary">Save changes</button>
					<button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
				</div>
			</div>
		</div>
	</div>
	);
}