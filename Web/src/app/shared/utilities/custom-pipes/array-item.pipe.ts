import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
    name: 'arrayItem'
})

export class ArrayItemPipe implements PipeTransform {
    transform(items: any[], args: any): any {
        return items[args];
    }
}