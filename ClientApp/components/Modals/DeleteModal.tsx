import * as React from 'react';
import {Button, Message, Modal,Input, Dimmer,Loader, Segment} from "semantic-ui-react";


export interface DeleteModalProps {
    onSubmit: (itemId: string)=> Promise<Response>;
    successCallback: () => any;
    triggerElement: (name: string, clickFn:(e:React.SyntheticEvent<any>)=> void) => JSX.Element;
}

export interface DeleteModalState {
    itemId: string;
    modalOpen: boolean;
    error: string;
    loading: boolean;
}

export default class DeleteModal extends React.Component<DeleteModalProps, DeleteModalState> {
    constructor() {
        super();
        this.state = { 
            modalOpen: false,
            loading: false,
            error: "",
            itemId: ""
        };
    }

    public handleInputChange = (e : React.SyntheticEvent<any>, {name,value}: any) => {
        this.setState({itemId: value})
    }

   

    public canSubmit() : boolean{
        if(!this.state.itemId ||this.state.itemId.length != 10){
            return false;
        }
        return true;
    }

    private handleOpen = (e : React.SyntheticEvent<any>) => {
        e.stopPropagation();
        e.preventDefault();

        this.setState({ modalOpen: true });
    };

    private handleClose = () => this.setState( { 
        modalOpen: false,
        loading: false,
        error: "",
        itemId: ""
    });

    private handleSubmission = () =>{
        this.setState({loading: true})
        this.props.onSubmit(this.state.itemId)
        .then(response =>{
            if(response.ok){
                this.props.successCallback();
                this.handleClose();
            }
            else{
                if(response.status === 404){
                    this.setState({error: "Item not found."})
                }
                else{
                    this.setState({error: "Unable to delete. " + response.statusText})
                }
                this.setState({loading: false})
            }
        })
        .catch(ex=>{
            this.setState({error: "Unable to delete. "});
            this.setState({loading: false})
        });
    }

    private renderErrorMessage(){
        if(this.state.error != ""){
            return (<Message negative>{this.state.error}</Message>)
        }
        return ""
    }

    public render() {
        return(
            <Modal className="scrolling" trigger={this.props.triggerElement("Delete Item", this.handleOpen)} open={this.state.modalOpen} onClose={this.handleClose}>
                <Modal.Header>Delete an Item</Modal.Header>
                <Segment basic>
                        <Dimmer active={this.state.loading}>
                            <Loader />
                        </Dimmer>
                        {this.renderErrorMessage()}
                        <div className="FlexContent">
                            <Input value={this.state.itemId} onChange={this.handleInputChange} name="Item Id" label="Id"></Input>
                            <div>
                                <Button disabled={!this.canSubmit()} onClick={this.handleSubmission}>Submit</Button>
                                <Button onClick={this.handleClose}>Cancel</Button>
                            </div>
                        </div>
                </Segment>
            </Modal>)
    }
}
