import * as React from 'react';
import {Button, Message, Modal,Input, Dimmer,Loader, Segment} from "semantic-ui-react";
import {Item} from "../../Services/ItemService";

export interface AddModalProps {
    onSubmit: (name: string,phone: string)=> Promise<Response>;
    successCallback: () => any;
    triggerElement: (name: string, clickFn:(e:React.SyntheticEvent<any>)=> void) => JSX.Element;
}

export interface AddModalState {
    item: Item;
    modalOpen: boolean;
    error: string;
    loading: boolean;
}

//Todo: Get Id back for display
export default class AddModal extends React.Component<AddModalProps, AddModalState> {
    constructor() {
        super();
        this.state = { 
            modalOpen: false,
            loading: false,
            error: "",
            item :{Name: "", PhoneNumber: "" } as Item
        };
    }

    public handleInputChange = (e : React.SyntheticEvent<any>, {name,value}: any) => {
        const clone = {...this.state.item} as any;
        clone[name] = value;
        this.setState({item: clone})
    }

    public validPhoneFormat(): boolean{
        debugger;
        var digitsOnly = this.state.item.PhoneNumber.replace(/\D/g,'');
        if(digitsOnly.length === 10){
            return true
        }
        return false;
    }

    public canSubmit() : boolean{
        if(!this.state.item.PhoneNumber ||this.state.item.PhoneNumber == "" || !this.state.item.Name ||this.state.item.Name == "" || !this.validPhoneFormat()){
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
        item :{Name: "", PhoneNumber: "" } as Item
    });

    private handleSubmission = () =>{
        this.setState({loading: true})
        this.props.onSubmit(this.state.item.Name,this.state.item.PhoneNumber)
        .then(response =>{
            if(response.ok){
                this.props.successCallback();
                this.handleClose();
            }
            else{
                this.setState({error: "Submission Failed. " + response.statusText})
                this.setState({loading: false})
            }
        })
        .catch(ex=>{
            this.setState({error: "Submission Failed. "});
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
            <Modal className="scrolling" trigger={this.props.triggerElement("Add Item", this.handleOpen)} open={this.state.modalOpen} onClose={this.handleClose}>
                <Modal.Header>Create an Item</Modal.Header>
                <Segment basic>
                        <Dimmer active={this.state.loading}>
                            <Loader />
                        </Dimmer>
                        {this.renderErrorMessage()}
                        <div className="FlexContent">
                            <Input value={this.state.item.Name} onChange={this.handleInputChange} name="Name" label="Name"></Input>
                            <Input value={this.state.item.PhoneNumber} onChange={this.handleInputChange} name="PhoneNumber" label="Phone #"></Input>
                            <div>
                                <Button disabled={!this.canSubmit()} onClick={this.handleSubmission}>Submit</Button>
                                <Button onClick={this.handleClose}>Cancel</Button>
                            </div>
                        </div>
                       
               
                </Segment>
            </Modal>)
    }
}
