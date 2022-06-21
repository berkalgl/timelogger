import React from 'react';
import { configure, shallow } from 'enzyme';
import Table from './Table';
import Modal from './Modal';
import Adapter from 'enzyme-adapter-react-16';

configure({ adapter: new Adapter() });

describe('<Table />', () => {
    const wrapper = shallow((
        <Table
            data={[{ id: '1', name: 'name' }, { id: '2', name: 'name2' }]}
            columns={[{
                dataField: 'id',
                text: '#',
                keyField: true
            }, {
                dataField: 'name',
                text: 'name'
            }]}
            parentIdName="id"
            parentId={1}
            isAdd={true}
            addForm={<Modal />}
        />
    ));
    it('should have table attribute', () => {
        expect(wrapper.find('table').length).toEqual(1);
    });

    it('should have thead', () => {
        expect(wrapper.find('thead').length).toEqual(1);
    });
    it('should have 2 columns and 3 rows', () => {
        expect(wrapper.find('th').length).toEqual(3);
        expect(wrapper.find('tr').length).toEqual(3);
    });

    it('should have tbody', () => {
        expect(wrapper.find('tbody').length).toEqual(1);
    });

    it('should have set addForm open to true', () => {
        expect(wrapper.find('CommonModal').prop('showModal')).toBeFalsy();
        wrapper.find('button#id1').simulate('click');
        expect(wrapper.find('CommonModal').prop('showModal')).toBeTruthy()
    });
    
})

