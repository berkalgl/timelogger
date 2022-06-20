import React from 'react';
import { configure, shallow } from 'enzyme';
import Timesheets from './Timesheets';
import Adapter from 'enzyme-adapter-react-16';

configure({ adapter: new Adapter() });

describe('<Timesheets />', () => {

    const wrapper = shallow((
        <Timesheets
        parentId={1}
        />
    ));
    console.log(wrapper.debug())

    it('should have Table', () => {
        expect(wrapper.find('Table').length).toEqual(1);
    });

    it('should have columns length 4', () => {
        expect(wrapper.find('Table').prop('columns').length).toEqual(4);
    });

    it('should have parent id 1', () => {
        expect(wrapper.find('Table').prop('parentId')).toBe(1);
    });
})

