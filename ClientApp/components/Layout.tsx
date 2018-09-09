import * as React from 'react';
import { NavMenu } from './NavMenu';
import 'semantic-ui-css/semantic.min.css';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return (
        <div className='container-fluid'>
       
            <NavMenu />
              
            <div className='row PageContent'>                
                <div className='col-sm-12'>
                    { this.props.children }
                </div>
            </div>
        </div>);
    }
}
