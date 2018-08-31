import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'enumKeyValuelist'
})

export class EnumKeyValueListPipe implements PipeTransform {
    transform(value: any, args: any[]): any {
        let items: any[] = [];
        for (let key in value){
            var isValueProperty = parseInt(key, 10) >= 0;
            if(!isValueProperty) continue;
            items.push({key: key, value: value[key]});
        }
        return items;
    }
}