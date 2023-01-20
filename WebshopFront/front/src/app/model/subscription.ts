import { PaymentMicroserviceType } from "../enums/payment-microservice-type";
import { Merchant } from "./merchant";

export interface Subscription {
    merchant: Merchant;
    paymentMicroserviceType: PaymentMicroserviceType;
}
